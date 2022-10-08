using SqlSugar;
using System.Linq.Expressions;
using Zhzt.Exam.Document.DomainInterface;
using Zhzt.Exam.Document.DomainModel;

namespace Zhzt.Exam.DocumentLib.Api.Models
{
    public class FileDocumentFilter
    {
        public int PageIndex { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public long CategoryId { get; set; } = 0;

        /// <summary>
        /// 对象转表达式
        /// </summary>
        /// <returns></returns>
        public Expression<Func<FileDocument, bool>> GetFilterExpression(IDocumentCategoryService? service)
        {
            List<long> matchIds = new List<long>();
            matchIds.Add(CategoryId);
            var allChilds = service?.GetAllChildren<DocumentCategory>(CategoryId);
            if (allChilds?.Count() > 0)
            {
                matchIds = matchIds.Concat(allChilds.Select(x => x.Id).ToList()).ToList();
            }
            return Expressionable.Create<FileDocument>()
                .AndIF(CategoryId != 0, l => matchIds.Contains(l.CategoryId))
                .ToExpression();
        }
    }
}
