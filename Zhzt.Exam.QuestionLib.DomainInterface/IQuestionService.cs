using SqlSugar.Extensions.DomainHelper;
using Zhzt.Exam.QuestionLib.DomainModel;

namespace Zhzt.Exam.QuestionLib.DomainInterface
{
    public interface IQuestionService : IBaseService
    {
        // 附加分类信息到实体
        public void AttachQuestionType(IEnumerable<Question>? questions);
        public void AttachQuestionType(Question? question);
    }
}
