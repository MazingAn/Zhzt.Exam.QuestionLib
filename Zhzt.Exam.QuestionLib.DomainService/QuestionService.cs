using MySqlX.XDevAPI.Common;
using Netcore.Extensions.WebModels;
using SqlSugar;
using SqlSugar.Extensions.DomainHelper;
using System.Linq.Expressions;
using Zhzt.Exam.QuestionLib.DomainInterface;
using Zhzt.Exam.QuestionLib.DomainModel;

namespace Zhzt.Exam.QuestionLib.DomainService
{
    public class QuestionService : BaseService, IQuestionService
    {
        // Invoke super consturction for denpendency reject
        // 显示调用父类构造函数完成注入
        public QuestionService(ISqlSugarClient client) : base(client)
        {
        }

        /// <summary>
        /// 附加分类信息
        /// </summary>
        /// <param name="questions"></param>
        public void AttachQuestionType(IEnumerable<Question>? questions)
        {
            if(questions != null)
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
            if(question != null)
            {
                var questionType = _client?.Queryable<QuestionType>().WithCache().Where(qt => qt.Id == question.QuestionTypeId).Single();
                question.QuestionType = questionType;
            }
        }

    }
}