using SqlSugar;
using System.Linq.Expressions;
using Zhzt.Exam.QuestionLib.DomainModel;

namespace Zhzt.Exam.QuestionLib.Api.Models
{
    public class QuestionFilter
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int QuestionClass { get; set; }

        public long QuestionTypeId { get; set; }

        /// <summary>
        /// 对象转表达式
        /// </summary>
        /// <returns></returns>
        public Expression<Func<Question, bool>> GetFilterExpression()
        {
            return Expressionable.Create<Question>()
                .AndIF(QuestionClass != 0, l => l.QuestionClass == QuestionClass)
                .AndIF(QuestionTypeId != 0, l => l.QuestionTypeId == QuestionTypeId)
                .ToExpression();
        }
    }
}
