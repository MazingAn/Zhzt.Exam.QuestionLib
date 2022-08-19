using MongoDb.Extensions.DomainHelper;
using Zhzt.Exam.PaperLib.DomainModel;

namespace Zhzt.Exam.PaperLib.DomainInterface
{
    public interface IPaperService : IBaseService<DocPaper>
    {
        /// <summary>
        /// 更新试卷
        /// </summary>
        /// <param name="paper">试卷信息</param>
        /// <returns></returns>
        public DocPaper UpdatePaper(DocPaper paper);

        /// <summary>
        /// 生成试卷
        /// </summary>
        /// <param name="docPaper">试卷信息，必须包含配置</param>
        /// <returns></returns>
        public DocPaper GenerateQuestions(DocPaper docPaper);
    }
}