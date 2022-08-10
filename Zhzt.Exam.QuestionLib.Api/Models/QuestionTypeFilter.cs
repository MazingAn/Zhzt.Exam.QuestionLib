using SqlSugar;
using System.Linq.Expressions;
using Zhzt.Exam.QuestionLib.DomainModel;

namespace Zhzt.Exam.QuestionLib.Api.Models
{
    public class QuestionTypeFilter
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public long ParentId { get; set; } = -1;

        /// <summary>
        /// 对象转表达式
        /// </summary>
        /// <returns></returns>
        public Expression<Func<QuestionType, bool>> GetFilterExpression()
        {
            return Expressionable.Create<QuestionType>()
                .AndIF(ParentId != -1, l => l.ParentId == ParentId)
                .ToExpression();
        }
    }
}
