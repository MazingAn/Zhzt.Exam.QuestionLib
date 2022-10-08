using SqlSugar;
using SqlSugar.Extension.DomainHelper;
using Zhzt.Exam.Document.DomainInterface;

namespace Zhzt.Exam.Document.DomainService
{
    public class DocumentCategoryService : BaseCachedService, IDocumentCategoryService
    {
        public DocumentCategoryService(ISqlSugarClient client) : base(client)
        {
        }
    }
}
