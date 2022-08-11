using Microsoft.AspNetCore.Http;
using SqlSugar.Extension.DomainHelper;
using Zhzt.Exam.QuestionLib.DomainModel;

namespace Zhzt.Exam.QuestionLib.DomainInterface
{
    public interface IQuestionService : IBaseService
    {
        // 附加分类信息到实体
        public void AttachQuestionType(IEnumerable<Question>? questions);
        public void AttachQuestionType(Question? question);

        // 批量导入试题
        public int Import(long questionTypeId, IFormFile upFile);
    }
}
