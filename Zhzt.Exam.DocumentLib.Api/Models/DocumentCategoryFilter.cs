using SqlSugar;
using System.Linq.Expressions;
using Zhzt.Exam.Document.DomainModel;

namespace Zhzt.Exam.DocumentLib.Api.Models
{
    public class DocumentCategoryFilter
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public long ParentId { get; set; } = -1;

        /// <summary>
        /// 对象转表达式
        /// </summary>
        /// <returns></returns>
        public Expression<Func<DocumentCategory, bool>> GetFilterExpression()
        {
            return Expressionable.Create<DocumentCategory>()
                .AndIF(ParentId != -1, l => l.ParentId == ParentId)
                .ToExpression();
        }
    }
}
