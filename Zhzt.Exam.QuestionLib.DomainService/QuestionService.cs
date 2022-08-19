using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SqlSugar;
using SqlSugar.Extension.DomainHelper;
using System.Drawing;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using Zhzt.Exam.QuestionLib.DomainInterface;
using Zhzt.Exam.QuestionLib.DomainModel;
using Zhzt.Exam.StaticFileSystem;

namespace Zhzt.Exam.QuestionLib.DomainService
{
    public class QuestionService : BaseService, IQuestionService
    {

        private Dictionary<string, int> _questionClassMapper;

        private readonly FileSystemSettings _staticFileSettings;


        // Invoke super consturction for denpendency reject
        // 显示调用父类构造函数完成注入
        public QuestionService(ISqlSugarClient client, IOptions<FileSystemSettings> staticFIleSettings) : base(client)
        {
            _questionClassMapper = new Dictionary<String, int>()
            {
                { "单选题", 1},{ "多选题", 2},{ "判断题", 3},
                { "填空题", 4},{ "问答题", 5},{ "名词解释题", 6},
                { "计算题", 7},{ "论述题", 8},{ "未知", -1}
            };
            _staticFileSettings = staticFIleSettings.Value;
        }

        /// <summary>
        /// 附加分类信息
        /// </summary>
        /// <param name="questions"></param>
        public void AttachQuestionType(IEnumerable<Question>? questions)
        {
            if (questions != null)
            {
                foreach (var item in questions)
                {
                    AttachQuestionType(item);
                }
            }
        }

        /// <summary>
        /// 附加分类信息
        /// </summary>
        /// <param name="questions"></param>
        public void AttachQuestionType(Question? question)
        {
            if (question != null)
            {
                var cachedQuestionType = _client?.DataCache.Get<QuestionType>(question.QuestionTypeId.ToString());
                if(cachedQuestionType != null)
                {
                    question.QuestionType = cachedQuestionType;
                }
                else
                {
                    var questionType = _client?.Queryable<QuestionType>().Where(x => x.Id == question.QuestionTypeId).Single();
                    var questionTypeChilds = _client?.Queryable<QuestionType>().ToTree(it => it.Child, it => it.ParentId, question.QuestionTypeId);
                    if (questionType != null)
                    {
                        questionType.Child = questionTypeChilds ?? new();
                        _client?.DataCache.Add(question.QuestionTypeId.ToString(), questionType);
                    }
                    question.QuestionType = questionType;
                }
            }
        }

        /// <summary>
        /// 随机再过滤之后获取多条数据
        /// </summary>
        /// <param name="filter">过滤内容</param>
        /// <param name="size">数量</param>
        /// <returns></returns>
        public IEnumerable<Question> GetRandomQuestions(Expression<Func<Question, bool>> filter, int size)
        {
            try
            {
                var result = _client?.Queryable<Question>().Where(filter)
                    .Take(size).OrderBy(st => SqlFunc.GetRandom()).ToList();
                if(result?.Count < size)
                {
                    throw new Exception("题库数量不足，无法完成抽取！");
                }
                if(result != null)
                {
                    AttachQuestionType(result);
                    return result;
                }
                else
                {
                    throw new Exception("抽取数据失败");
                }
            }
            catch
            {
                throw new Exception("题库抽取失败，请检查参数并确认题库中数据量足够支撑完成抽取");
            }
        }

        /// <summary>
        /// 从文件导入
        /// </summary>
        /// <param name="questionTypeId"></param>
        /// <param name="upFile"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int Import(long questionTypeId, IFormFile upFile)
        {
            string extension = Path.GetExtension(upFile.FileName);
            List<Question> fileData = new();
            switch (extension)
            {
                case ".xls":
                    fileData = LoadFromExcel(upFile.OpenReadStream(), "xls", questionTypeId);
                    break;
                case ".xlsx":
                    fileData = LoadFromExcel(upFile.OpenReadStream(), "xlsx", questionTypeId);
                    break;
                case ".docx":
                    var tempPath = Path.Combine(_staticFileSettings.StaticServerRoot, Guid.NewGuid().ToString() + ".docx");
                    using (var fs = System.IO.File.Create(tempPath))
                    {
                        upFile.CopyTo(fs);
                        fs.Flush();
                    }
                    fileData = LoadFromWord(tempPath, questionTypeId);
                    File.Delete(tempPath);
                    break;
                default:
                    break;
            }
            _client?.Ado.BeginTran();
            foreach (var ques in fileData)
            {
                Save<Question>(ques);
            }
            _client?.Ado.CommitTran();
            return fileData.Count();
        }

        /// <summary>
        /// 从excel读取题库数据
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="extension"></param>
        /// <param name="questionTypeId"></param>
        /// <returns></returns>
        private List<Question> LoadFromExcel(Stream stream, string extension, long questionTypeId)
        {
            List<Question> questions = new List<Question>();
            IWorkbook workbook = extension == ".xlsx" ? (IWorkbook)new XSSFWorkbook(stream) : new HSSFWorkbook(stream);
            ISheet sheet = workbook.GetSheetAt(0);
            if (sheet != null)
            {
                int rowCount = sheet.LastRowNum;
                for (int i = 1; i <= rowCount; i++)
                {
                    IRow row = sheet.GetRow(i);
                    int quesClass = _questionClassMapper[row.GetCell(8).ToString() ?? "未知"];
                    string rightAnswer = ParseAnswers(row);
                    Question ques = new Question
                    {
                        QuestionBody = row.GetCell(0).ToString() ?? "错误",
                        QuestionTypeId = questionTypeId,
                        Answer1 = row.GetCell(1).ToString()?.Trim() ?? "",
                        Answer2 = row.GetCell(2).ToString()?.Trim() ?? "",
                        Answer3 = row.GetCell(3).ToString()?.Trim() ?? "",
                        Answer4 = row.GetCell(4).ToString()?.Trim() ?? "",
                        Answer5 = row.GetCell(5).ToString()?.Trim() ?? "",
                        Answer6 = row.GetCell(6).ToString()?.Trim() ?? "",
                        RightAnswer = rightAnswer,
                        QuestionClass = quesClass,
                    };
                    if (ques.QuestionBody != "错误" && ques.RightAnswer != "错误")
                    {
                        questions.Add(ques);
                    }
                }
            }
            return questions;
        }

        /// <summary>
        /// 处理不规范的答案
        /// </summary>
        /// <param name="answer"></param>
        /// <returns></returns>
        private string ParseAnswers(IRow row)
        {
            int quesClass = _questionClassMapper[row.GetCell(8).ToString() ?? "未知"];
            string answer = row.GetCell(7).ToString() ?? "错误";

            switch (quesClass)
            {
                case -1:
                    return "错误";
                case 1:
                    return answer.Trim().ToUpper();
                case 2:
                    List<string> mcAnswers = new();
                    foreach (char ch in answer.ToUpper())
                    {
                        if ("ABCDEF".Contains(ch))
                        {
                            mcAnswers.Add(ch.ToString());
                        }
                    }
                    return string.Join('$', mcAnswers);
                case 3:
                    return (answer == "对").ToString().ToLower();
                case 4:
                    List<string> bfAnswers = new();
                    for (int i = 1; i < 6; i++)
                    {
                        var ans = row.GetCell(i).ToString()?.Trim() ?? string.Empty;
                        if (ans != string.Empty)
                        {
                            bfAnswers.Add(ans);
                        }
                    }
                    return string.Join('$', bfAnswers);
                default:
                    return answer;
            }
        }

        /// <summary>
        /// 从word导入数据
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="questionTypeId"></param>
        /// <returns></returns>
        private List<Question> LoadFromWord(string filePath, long questionTypeId)
        {
            List<string> lines = ReadWordLines(filePath);
            // 逐行按照规则解析word数据
            lines = lines.Select(x => x.Trim()).ToList();
            string quesTypeName = null!;
            Question ques = null!;
            List<Question> questions = new();
            // 逐行读取 导入题库
            for (int i = 0; i < lines.Count; i++)
            {
                lines[i].Replace("\n", String.Empty);
                if (Regex.IsMatch(lines[i], "^[一二三四五六七八九十]、"))
                {
                    quesTypeName = lines[i].Substring(lines[i].IndexOf("、") + 1, lines[i].Length - 1 - lines[i].IndexOf("、"));
                }

                if (Regex.IsMatch(lines[i], "^\\d+\\."))
                {
                    if (ques != null)
                    {
                        ques.QuestionTypeId = questionTypeId;
                        questions.Add(ques);
                    }
                    ques = new Question();
                    // 获取题干
                    var quesBody = lines[i].Substring(lines[i].IndexOf(".") + 1, lines[i].Length - lines[i].IndexOf(".") - 1);
                    ques.QuestionBody = quesBody;
                    if (!string.IsNullOrEmpty(quesTypeName))
                    {
                        if (quesTypeName.Contains("单项选择"))
                        {
                            ques.QuestionClass = 1;
                            for (int j = i + 1; j < lines.Count; j++)
                            {
                                // 已经到了答案部分了
                                if (Regex.IsMatch(lines[j], "^【答案】"))
                                {
                                    i = j - 1;
                                    break;
                                }
                                // 匹配到接下来时空行
                                if (string.IsNullOrEmpty(lines[j]))
                                {
                                    continue;
                                }
                                // 匹配到选项
                                if (lines[j].Contains("A："))
                                {
                                    if (lines[j].Contains("B："))
                                    {
                                        int startIndex = lines[j].IndexOf("A：") + 2;
                                        int endIndex = lines[j].IndexOf("B：");
                                        int length = endIndex - startIndex;
                                        ques.Answer1 = lines[j].Substring(startIndex, length).Trim();
                                    }
                                    else
                                    {
                                        int startIndex = lines[j].IndexOf("A：") + 2;
                                        int length = lines[j].Length - startIndex;
                                        ques.Answer1 = lines[j].Substring(startIndex, length).Trim();
                                    }
                                }
                                if (lines[j].Contains("B："))
                                {
                                    if (lines[j].Contains("C："))
                                    {
                                        int startIndex = lines[j].IndexOf("B：") + 2;
                                        int endIndex = lines[j].IndexOf("C：");
                                        int length = endIndex - startIndex;
                                        ques.Answer2 = lines[j].Substring(startIndex, length).Trim();
                                    }
                                    else
                                    {
                                        int startIndex = lines[j].IndexOf("B：") + 2;
                                        int length = lines[j].Length - startIndex;
                                        ques.Answer2 = lines[j].Substring(startIndex, length).Trim();
                                    }
                                }
                                if (lines[j].Contains("C："))
                                {
                                    if (lines[j].Contains("D："))
                                    {
                                        int startIndex = lines[j].IndexOf("C：") + 2;
                                        int endIndex = lines[j].IndexOf("D：");
                                        int length = endIndex - startIndex;
                                        ques.Answer3 = lines[j].Substring(startIndex, length).Trim();
                                    }
                                    else
                                    {
                                        int startIndex = lines[j].IndexOf("C：") + 2;
                                        int length = lines[j].Length - startIndex;
                                        ques.Answer3 = lines[j].Substring(startIndex, length).Trim();
                                    }
                                }
                                if (lines[j].Contains("D："))
                                {
                                    if (lines[j].Contains("E："))
                                    {
                                        int startIndex = lines[j].IndexOf("D：") + 2;
                                        int endIndex = lines[j].IndexOf("E：");
                                        int length = endIndex - startIndex;
                                        ques.Answer4 = lines[j].Substring(startIndex, length).Trim();
                                    }
                                    else
                                    {
                                        int startIndex = lines[j].IndexOf("D：") + 2;
                                        int length = lines[j].Length - startIndex;
                                        ques.Answer4 = lines[j].Substring(startIndex, length).Trim();
                                    }
                                }
                                if (lines[j].Contains("E："))
                                {
                                    if (lines[j].Contains("F："))
                                    {
                                        int startIndex = lines[j].IndexOf("E：") + 2;
                                        int endIndex = lines[j].IndexOf("F：");
                                        int length = endIndex - startIndex;
                                        ques.Answer5 = lines[j].Substring(startIndex, length).Trim();
                                    }
                                    else
                                    {
                                        int startIndex = lines[j].IndexOf("E：") + 2;
                                        int length = lines[j].Length - startIndex;
                                        ques.Answer5 = lines[j].Substring(startIndex, length).Trim();
                                    }
                                }
                                if (lines[j].Contains("F："))
                                {
                                    int startIndex = lines[j].IndexOf("F：") + 2;
                                    int length = lines[j].Length - startIndex;
                                    ques.Answer6 = lines[j].Substring(startIndex, length).Trim();
                                }
                            }
                        }
                        if (quesTypeName.Contains("多项选择"))
                        {
                            ques.QuestionClass = 2;
                            for (int j = i + 1; j < lines.Count; j++)
                            {
                                // 已经到了下一题的边界了
                                if (Regex.IsMatch(lines[j], "^【答案】"))
                                {
                                    i = j - 1;
                                    break;
                                }
                                // 匹配到接下来时空行
                                if (string.IsNullOrEmpty(lines[j]))
                                {
                                    continue;
                                }
                                // 匹配到选项
                                if (lines[j].Contains("A："))
                                {
                                    if (lines[j].Contains("B："))
                                    {
                                        int startIndex = lines[j].IndexOf("A：") + 2;
                                        int endIndex = lines[j].IndexOf("B：");
                                        int length = endIndex - startIndex;
                                        ques.Answer1 = lines[j].Substring(startIndex, length).Trim();
                                    }
                                    else
                                    {
                                        int startIndex = lines[j].IndexOf("A：") + 2;
                                        int length = lines[j].Length - startIndex;
                                        ques.Answer1 = lines[j].Substring(startIndex, length).Trim();
                                    }
                                }
                                if (lines[j].Contains("B："))
                                {
                                    if (lines[j].Contains("C："))
                                    {
                                        int startIndex = lines[j].IndexOf("B：") + 2;
                                        int endIndex = lines[j].IndexOf("C：");
                                        int length = endIndex - startIndex;
                                        ques.Answer2 = lines[j].Substring(startIndex, length).Trim();
                                    }
                                    else
                                    {
                                        int startIndex = lines[j].IndexOf("B：") + 2;
                                        int length = lines[j].Length - startIndex;
                                        ques.Answer2 = lines[j].Substring(startIndex, length).Trim();
                                    }
                                }
                                if (lines[j].Contains("C："))
                                {
                                    if (lines[j].Contains("D："))
                                    {
                                        int startIndex = lines[j].IndexOf("C：") + 2;
                                        int endIndex = lines[j].IndexOf("D：");
                                        int length = endIndex - startIndex;
                                        ques.Answer3 = lines[j].Substring(startIndex, length).Trim();
                                    }
                                    else
                                    {
                                        int startIndex = lines[j].IndexOf("C：") + 2;
                                        int length = lines[j].Length - startIndex;
                                        ques.Answer3 = lines[j].Substring(startIndex, length).Trim();
                                    }
                                }
                                if (lines[j].Contains("D："))
                                {
                                    if (lines[j].Contains("E："))
                                    {
                                        int startIndex = lines[j].IndexOf("D：") + 2;
                                        int endIndex = lines[j].IndexOf("E：");
                                        int length = endIndex - startIndex;
                                        ques.Answer4 = lines[j].Substring(startIndex, length).Trim();
                                    }
                                    else
                                    {
                                        int startIndex = lines[j].IndexOf("D：") + 2;
                                        int length = lines[j].Length - startIndex;
                                        ques.Answer4 = lines[j].Substring(startIndex, length).Trim();
                                    }
                                }
                                if (lines[j].Contains("E："))
                                {
                                    if (lines[j].Contains("F："))
                                    {
                                        int startIndex = lines[j].IndexOf("E：") + 2;
                                        int endIndex = lines[j].IndexOf("F：");
                                        int length = endIndex - startIndex;
                                        ques.Answer5 = lines[j].Substring(startIndex, length).Trim();
                                    }
                                    else
                                    {
                                        int startIndex = lines[j].IndexOf("E：") + 2;
                                        int length = lines[j].Length - startIndex;
                                        ques.Answer5 = lines[j].Substring(startIndex, length).Trim();
                                    }
                                }
                                if (lines[j].Contains("F："))
                                {
                                    int startIndex = lines[j].IndexOf("F：") + 2;
                                    int length = lines[j].Length - startIndex;
                                    ques.Answer6 = lines[j].Substring(startIndex, length).Trim();
                                }
                            }
                        }
                        if (quesTypeName.Contains("判断"))
                        {
                            ques.QuestionClass = 3;
                        }
                        if (quesTypeName.Contains("填空"))
                        {
                            ques.QuestionClass = 4;
                        }
                        if (quesTypeName.Contains("问答"))
                        {
                            ques.QuestionClass = 5;
                        }
                        if (quesTypeName.Contains("名词解释"))
                        {
                            ques.QuestionClass = 6;
                        }
                        if (quesTypeName.Contains("计算"))
                        {
                            ques.QuestionClass = 7;
                        }
                        if (quesTypeName.Contains("论述"))
                        {
                            ques.QuestionClass = 8;
                        }
                    }
                }
                // 获取答案
                if (Regex.IsMatch(lines[i], "^【答案】"))
                {
                    if (quesTypeName.Contains("单项选择"))
                    {
                        ques.RightAnswer = lines[i].Replace("【答案】", "");
                    }
                    else if (quesTypeName.Contains("多项选择"))
                    {
                        List<string> answers = new();
                        foreach (char ch in lines[i].Replace("【答案】", ""))
                        {
                            if ("ABCDEF".Contains(ch))
                            {
                                answers.Add(ch.ToString());
                            }
                        }
                        ques.RightAnswer = String.Join('$', answers);
                    }
                    else if (quesTypeName.Contains("计算") || quesTypeName.Contains("填空"))
                    {
                        ques.RightAnswer = String.Join('$', lines[i].Replace("【答案】", "").Split('$'));
                    }
                    else
                    {
                        ques.RightAnswer = lines[i].Replace("【答案】", "");
                    }
                }
            }
            // 最后一题追加一下
            ques.QuestionTypeId = questionTypeId;
            questions.Add(ques);
            return questions;
        }

        /// <summary>
        /// word转换成字符串数组
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private List<string> ReadWordLines(string filePath)
        {
            if (!System.IO.Directory.Exists(_staticFileSettings.StaticServerRoot))
            {
                System.IO.Directory.CreateDirectory(_staticFileSettings.StaticServerRoot);
            }
            List<string> lines = new List<string>();
            using (WordprocessingDocument doc = WordprocessingDocument.Open(filePath, true))
            {
                Body body = doc.MainDocumentPart!.Document.Body!;
                foreach (var paragraph in body.Elements<Paragraph>())
                {
                    StringBuilder builder = new StringBuilder();
                    foreach (var run in paragraph.Elements<Run>())
                    {
                        foreach (OpenXmlElement openXmlElement in run.Elements())
                        {
                            if (openXmlElement is Text text)
                            {
                                builder.Append(text.Text);
                            }
                            //嵌入对象的导入
                            else if (openXmlElement is EmbeddedObject emb) 
                            {
                                foreach (OpenXmlElement element in emb.Elements())
                                {
                                    // wmf格式的图片
                                    if (element is DocumentFormat.OpenXml.Vml.Shape shape)
                                    {
                                        var imgDatas = shape.Elements<DocumentFormat.OpenXml.Vml.ImageData>();
                                        foreach (var imgData in imgDatas)
                                        {
                                            var fileName = $"{Guid.NewGuid().ToString()}.png";
                                            var imgUrl = Path.Combine(_staticFileSettings.StaticServerRoot, fileName);
                                            var part = doc.MainDocumentPart.GetPartById(imgData.RelationshipId!);
                                            Console.WriteLine(part.ContentType);
                                            if (part.ContentType == "image/x-wmf")
                                            {
                                                Stream imgstream = part.GetStream();
                                                Bitmap bitmap = new Bitmap(imgstream);
                                                bitmap.Save(imgUrl, System.Drawing.Imaging.ImageFormat.Png);
                                                bitmap.Dispose();
                                                imgstream.Close();
                                                builder.Append($"<img src=\"{fileName}\"/>");
                                            }
                                        }
                                    }
                                }

                            }
                            // 正常嵌入的图片
                            else if (openXmlElement is Drawing drawing)
                            {
                                DocumentFormat.OpenXml.Drawing.GraphicData gdata = null!;
                                if (drawing.Inline != null)
                                {
                                    var inline = drawing.Inline;
                                    var extent = inline.Extent;
                                    gdata = inline.Graphic?.GraphicData;
                                }
                                else
                                {
                                    foreach (var drawEle in drawing.Anchor.ChildElements)
                                    {
                                        if (drawEle is DocumentFormat.OpenXml.Drawing.Graphic gp)
                                        {
                                            gdata = gp?.GraphicData;
                                            break;
                                        }
                                    }
                                }
                                if (gdata != null)
                                {
                                    var pic = gdata.GetFirstChild<DocumentFormat.OpenXml.Drawing.Pictures.Picture>();
                                    var embed = pic.BlipFill.Blip.Embed.Value!;
                                    //得到图像流
                                    var part = doc.MainDocumentPart.GetPartById(embed);
                                    var fileName = $"{Guid.NewGuid().ToString()}.png";
                                    var imgUrl = Path.Combine(_staticFileSettings.StaticServerRoot, fileName);
                                    Stream imgstream = part.GetStream();
                                    Bitmap bitmap = new Bitmap(imgstream);
                                    bitmap.Save(imgUrl, System.Drawing.Imaging.ImageFormat.Png);
                                    bitmap.Dispose();
                                    imgstream.Close();
                                    builder.Append($"<img src=\"{fileName}\"/>");
                                }
                            }
                        }
                    }
                    lines.Add(builder.ToString());
                }
                doc.Close();
            }
            return lines;
        }
    }
}