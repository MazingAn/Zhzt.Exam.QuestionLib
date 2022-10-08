using SqlSugar.Extension.DomainHelper;
using Zhzt.Exam.Document.DomainModel;

namespace Zhzt.Exam.Document.DomainInterface
{
    public interface IDocumentService : IBaseService
    {
        void AttachCategory(FileDocument doc);
        void AttachCategory(IEnumerable<FileDocument> docs);
        void GenThumb(string input, string output);
        void GenPdf(string input, string output);

        #region 开发选项
        void CreateSeedData();
        #endregion
    }
}