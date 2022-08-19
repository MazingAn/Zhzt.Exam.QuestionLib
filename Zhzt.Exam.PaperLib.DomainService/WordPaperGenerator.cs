using Amz.NPOIWord.Extension;
using Microsoft.Extensions.Options;
using MongoDb.Extensions.Options;
using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.SS.Formula.Functions;
using NPOI.XWPF.UserModel;
using Zhzt.Exam.PaperLib.Configuration;
using Zhzt.Exam.PaperLib.DomainInterface;
using Zhzt.Exam.PaperLib.DomainModel;

namespace Zhzt.Exam.PaperLib.DomainService
{
    /// <summary>
    /// Word试卷生成器
    /// </summary>
    public class WordPaperGenerator : IPaperGenerate
    {
        private readonly PaperGenerateSettings _genConf;

        private readonly string[] choiceLabel = "0 A B C D E F".Split();

        private readonly string[] numbersLabel = "零 一 二 三 四 五 六 七 八 九 十".Split();

        private XWPFDocument doc = new XWPFDocument();

        public WordPaperGenerator(IOptions<PaperGenerateSettings> generateConfig)
        {
            _genConf = generateConfig.Value;
        }

        /// <summary>
        /// 生成试卷
        /// </summary>
        /// <param name="paper"></param>
        /// <returns></returns>
        public string GeneratePaper(DocPaper paper)
        {
            try
            {
                if (_genConf.DefaultPaperSize == "A4")
                {
                    return GenerateA4Paper(paper);
                }
                else
                {
                    return GenerateA3Paper(paper);
                }
            }
            catch
            {
                throw new Exception("在生成word试卷时失败，请联系管理员！可能的问题:(1.路径配置错误 2.权限不足)");
            }
        }

        /// <summary>
        /// 生成A4试卷
        /// </summary>
        /// <param name="paper"></param>
        /// <returns></returns>
        private string GenerateA4Paper(DocPaper paper)
        {
            doc = doc.UseA4Size()
                .AddHeader(paper.Name, 26, 100, 100, _genConf.DefaultHeaderFontName, ParagraphAlignment.CENTER);

            // 生成题目打分表
            var paperheaderData = GenerateScoreTable(paper.PagerConfig);
            doc.AddTable(paperheaderData);

            // 生成题目
            for (int headerIdx = 0; headerIdx < paperheaderData.Headers.Count-1; headerIdx++)
            {
                // 单项选择
                if (paperheaderData.Headers[headerIdx].Contains("单项选择"))
                {
                    int count = paper.PagerConfig.SingleChoiceCount;
                    float totalScore = paper.PagerConfig.SingleChoiceTotalScore;
                    List<InnerDocPaperQuestion> questions = paper.SingleChoiceQuestions.ToList();
                    GenerateChoice(count, totalScore, questions, paperheaderData, headerIdx);
                }

                // 多项选择
                if (paperheaderData.Headers[headerIdx].Contains("多项选择"))
                {
                    int count = paper.PagerConfig.MultiChoiceCount;
                    float totalScore = paper.PagerConfig.MultiChoiceTotalScore;
                    List<InnerDocPaperQuestion> questions = paper.MultiChoiceQuestions.ToList();
                    GenerateChoice(count, totalScore, questions, paperheaderData, headerIdx);
                }

                // 判断题生成
                if (paperheaderData.Headers[headerIdx].Contains("判断"))
                {
                    int count = paper.PagerConfig.JudgeCount;
                    float totalScore = paper.PagerConfig.JudgeTotalScore;
                    List<InnerDocPaperQuestion> questions = paper.JudgeQuestions.ToList();
                    GenerateNormal(count, totalScore, questions, paperheaderData, headerIdx);
                }


                // 填空题生成
                if (paperheaderData.Headers[headerIdx].Contains("判断"))
                {
                    int count = paper.PagerConfig.BlankFillCount;
                    float totalScore = paper.PagerConfig.BlankFillTotalScore;
                    List<InnerDocPaperQuestion> questions = paper.BlankFillQuestions.ToList();
                    GenerateNormal(count, totalScore, questions, paperheaderData, headerIdx);
                }

                // 问答题生成
                if (paperheaderData.Headers[headerIdx].Contains("问答"))
                {
                    int count = paper.PagerConfig.QuesAnswerCount;
                    float totalScore = paper.PagerConfig.QuesAnswereTotalScore;
                    List<InnerDocPaperQuestion> questions = paper.QuesAnswerQuestions.ToList();
                    GenerateNormal(count, totalScore, questions, paperheaderData, headerIdx, 3);
                }
            }

            // 附加答案
            if (_genConf.AttachAnswers)
            {
                doc.BreakPage();
                doc = doc.AddHeader("参考答案", 18);
                foreach (var item in paperheaderData.Headers)
                {
                    if (item.Contains("单项选择"))
                    {
                        doc = doc.AddHeader(item, 14)
                            .AddContent(string.Join(' ', paper.SingleChoiceQuestions.Select((x,idx) => $"{idx+1}.{x.RightAnswer}").ToList()));
                    }
                    if (item.Contains("多项选择"))
                    {
                        doc = doc.AddHeader(item, 14)
                            .AddContent(string.Join(' ', paper.MultiChoiceQuestions.Select((x, idx) => $"{idx + 1}.{x.RightAnswer.Replace("$","")}").ToList()));
                    }
                    if (item.Contains("判断"))
                    {
                        doc = doc.AddHeader(item, 14)
                            .AddContent(string.Join(' ',
                                paper.JudgeQuestions.Select( (x,idx) =>$"{idx+1}.{x.RightAnswer.ToLower()}".Replace("true","对").Replace("false","错")).ToList()));
                    }
                    if (item.Contains("填空"))
                    {
                        doc = doc.AddHeader(item, 14);
                        var questions = paper.BlankFillQuestions.ToArray();
                        for (int i = 0; i < questions.Length; i++)
                        {
                            doc.AddContent($"{i + 1}.{questions[i].RightAnswer.Replace('$', '，')}");
                        }
                    }
                    if (item.Contains("问答")) 
                    {
                        doc = doc.AddHeader(item, 14);
                        var questions = paper.QuesAnswerQuestions.ToArray();
                        for (int i = 0; i < questions.Length; i++)
                        {
                            doc.AddContent($"{i + 1}.{questions[i].RightAnswer}");
                        }
                    }
                }
            }

            string filePath = Path.Combine(_genConf.BaseDir, $"{paper.Id}.docx");
            using (FileStream o = new FileStream(filePath, FileMode.Create))
            {
                doc.Write(o);
            }
            return $"{ paper.Id}.docx";
        }

        /// <summary>
        /// 生成A3试卷 没有实现
        /// </summary>
        /// <param name="paper"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private string GenerateA3Paper(DocPaper paper)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 选择题排版
        /// </summary>
        /// <param name="count"></param>
        /// <param name="totalScore"></param>
        /// <param name="questions"></param>
        /// <param name="questionTable"></param>
        /// <param name="headerIdx"></param>
        private void GenerateChoice(int count, float totalScore, List<InnerDocPaperQuestion> questions,
            CustomerNPOITableData questionTable, int headerIdx)
        {
            int scCount = count;
            double scPerScore = Math.Round((double)totalScore / (double)count, 2);
            doc = doc.AddHeader($"{questionTable.Headers[headerIdx]}:共{scCount}题,每题{scPerScore}分", 14, 100, 40, _genConf.DefaultHeaderFontName);
            var scarr = questions.ToArray();
            for (int i = 0; i < scarr.Length; i++)
            {
                var question = scarr[i];
                doc = doc.AddContent($"{i + 1}、{question.QuestionBody}", 50);
                var table = doc.CreateTable();
                table.Width = 5000;
                table.RemoveRow(0);
                table.SetTopBorder(XWPFTable.XWPFBorderType.NIL, 0, 0, "#FFFFFF");
                table.SetBottomBorder(XWPFTable.XWPFBorderType.NIL, 0, 0, "#FFFFFF");
                table.SetLeftBorder(XWPFTable.XWPFBorderType.NIL, 0, 0, "#FFFFFF");
                table.SetRightBorder(XWPFTable.XWPFBorderType.NIL, 0, 0, "#FFFFFF");
                table.SetInsideHBorder(XWPFTable.XWPFBorderType.NIL, 0, 0, "#FFFFFF");
                table.SetInsideVBorder(XWPFTable.XWPFBorderType.NIL, 0, 0, "#FFFFFF");
                table.GetCTTbl().AddNewTblPr().tblLayout = new CT_TblLayoutType() { type = ST_TblLayoutType.@fixed };
                for (int c = 1; c < 7; c++)
                {
                    var val = question.GetType().GetProperty($"Answer{c}")?.GetValue(question)?.ToString();
                    var nextVal = question.GetType().GetProperty($"Answer{c + 1}")?.GetValue(question)?.ToString();
                    // 处理数据短 可以两行并作一行的效果
                    if (!string.IsNullOrEmpty(val) && !string.IsNullOrEmpty(nextVal) && nextVal?.Length + val?.Length <= 40)
                    {
                        var ctrow = new CT_Row();
                        var mrow = new XWPFTableRow(ctrow, table);
                        mrow.AddNewTableCell(); 
                        mrow.AddNewTableCell();
                        mrow.GetCell(0).SetBorderBottom(XWPFTable.XWPFBorderType.NONE, 0,0, "#FFFFFF");
                        mrow.GetCell(1).SetBorderBottom(XWPFTable.XWPFBorderType.NONE, 0, 0, "#FFFFFF");
                        mrow.GetCell(0).SetText($"{ choiceLabel[c]}：{ val}");
                        mrow.GetCell(1).SetText($"{choiceLabel[c + 1]}：{nextVal}");
                        table.AddRow(mrow);
                        var cells = mrow.GetTableCells();
                        for (int col = 0; col < cells.Count; col++)
                        {
                            table.SetColumnWidth(col, 5 * 567);
                            table.SetCellMargins(50, 0, 50, 0);
                        }
                        c++;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(val))
                        {
                            var ctrow = new CT_Row();
                            var mrow = new XWPFTableRow(ctrow, table);
                            mrow.AddNewTableCell();
                            mrow.GetCell(0).SetBorderBottom(XWPFTable.XWPFBorderType.NONE, 0, 0, "#FFFFFF");
                            mrow.GetCell(0).SetText($"{choiceLabel[c]}：{val}");
                            table.AddRow(mrow);
                            var cells = mrow.GetTableCells();
                            for (int col = 0; col < cells.Count; col++)
                            {
                                table.SetColumnWidth(col, 5 * 567);
                                table.SetCellMargins(50, 0, 50, 0);
                            }
                        }
                    }
                }

            }
        }

        /// <summary>
        /// 普通题排版
        /// </summary>
        /// <param name="count"></param>
        /// <param name="totalScore"></param>
        /// <param name="questions"></param>
        /// <param name="questionTable"></param>
        /// <param name="headerIdx"></param>
        private void GenerateNormal(int count, float totalScore, List<InnerDocPaperQuestion> questions,
            CustomerNPOITableData questionTable, int headerIdx, int lineCount = 0)
        {
            int scCount = count;
            double scPerScore = Math.Round((double)totalScore / (double)count, 2);
            doc = doc.AddHeader($"{questionTable.Headers[headerIdx]}:共{scCount}题,每题{scPerScore}分", 14, 100, 40, _genConf.DefaultHeaderFontName);
            var scarr = questions.ToArray();
            for (int i = 0; i < scarr.Length; i++)
            {
                var question = scarr[i];
                doc = doc.AddContent($"{i + 1}、{question.QuestionBody}", 50);
                for (int j = 0; j < lineCount; j++)
                {
                    doc.AddContent("");
                }
            }
        }

        /// <summary>
        /// 试卷头部信息
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private CustomerNPOITableData GenerateScoreTable(InnerDocPagerConfig config)
        {
            CustomerNPOITableData data = new CustomerNPOITableData();
            int i = 1;
            List<string> headers = new();
            if (config.SingleChoiceCount > 0) headers.Add($"{numbersLabel[i++]}、单项选择题");
            if (config.MultiChoiceCount > 0) headers.Add($"{numbersLabel[i++]}、多项选择题");
            if (config.JudgeCount > 0) headers.Add($"{numbersLabel[i++]}、判断题");
            if (config.BlankFillCount > 0) headers.Add($"{numbersLabel[i++]}、填空题");
            if (config.QuesAnswerCount > 0) headers.Add($"{numbersLabel[i++]}、问答题");
            headers.Add($"总分");
            data.SetHeaders(headers);
            data.AppendRow(headers.Select(a=>" ").ToList());
            return data;
        }
    }
}
