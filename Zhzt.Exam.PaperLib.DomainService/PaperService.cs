using Microsoft.Extensions.Options;
using MongoDb.Extensions.DomainHelper;
using MongoDb.Extensions.Options;
using Zhzt.Exam.PaperLib.DomainInterface;
using Zhzt.Exam.PaperLib.DomainModel;

namespace Zhzt.Exam.PaperLib.DomainService
{
    public class PaperService : BaseService<DocPaper>, IPaperService
    {
        public PaperService(IOptions<MongoDbDataSettings> databaseSettings) : base(databaseSettings)
        {
        }
    }
}