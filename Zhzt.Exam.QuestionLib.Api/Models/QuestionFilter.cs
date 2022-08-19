using SqlSugar;
using System.Linq.Expressions;
using Zhzt.Exam.QuestionLib.DomainInterface;
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
        public Expression<Func<Question, bool>> GetFilterExpression(IQuestionTypeService? _service)
        {
            List<long> matchIds = new List<long>();
            matchIds.Add(QuestionTypeId);
            var allChilds = _service?.GetAllChildren<QuestionType>(QuestionTypeId);
            if (allChilds?.Count() > 0) {
                matchIds = matchIds.Concat(allChilds.Select(x => x.Id).ToList()).ToList();
            }
            return Expressionable.Create<Question>()
                .AndIF(QuestionClass != 0, l => l.QuestionClass == QuestionClass)
                .AndIF(QuestionTypeId != 0, l => matchIds.Contains(l.QuestionTypeId))
                .ToExpression();
        }
    }
}
