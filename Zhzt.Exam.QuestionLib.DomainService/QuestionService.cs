using Microsoft.AspNetCore.Http;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SqlSugar;
using SqlSugar.Extension.DomainHelper;
using Zhzt.Exam.QuestionLib.DomainInterface;
using Zhzt.Exam.QuestionLib.DomainModel;

namespace Zhzt.Exam.QuestionLib.DomainService
{
    public class QuestionService : BaseService, IQuestionService
    {

        private Dictionary<String, int> _questionClassMapper;

        // Invoke super consturction for denpendency reject
        // 显示调用父类构造函数完成注入
        public QuestionService(ISqlSugarClient client) : base(client)
        {
            _questionClassMapper = new Dictionary<String, int>()
            {
                { "单选题", 1},{ "多选题", 2},{ "判断题", 3},{ "填空题", 4},{ "问答题", 5},{ "未知", -1}
            };
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
                var questionType = _client?.Queryable<QuestionType>().WithCache().Where(qt => qt.Id == question.QuestionTypeId).Single();
                question.QuestionType = questionType;
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
            if(sheet != null)
            {
                int rowCount = sheet.LastRowNum;
                for(int i = 1; i <= rowCount; i++)
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
                    if(ques.QuestionBody != "错误" && ques.RightAnswer != "错误")
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
                        if(ans != string.Empty)
                        {
                            bfAnswers.Add(ans);
                        }
                    }
                    return string.Join('$', bfAnswers);
                default:
                    return answer;
            }
        }
    }
}