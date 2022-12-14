using SqlSugar;
using SqlSugar.Extension.DomainHelper;
using Zhzt.Exam.QuestionLib.DomainInterface;

namespace Zhzt.Exam.QuestionLib.DomainService
{
    /// <summary>
    /// This is a sample service
    /// </summary>
    public class QuestionTypeService : BaseCachedService, IQuestionTypeService
    {
        // Invoke super consturction for denpendency reject
        // 显示调用父类构造函数完成注入
        public QuestionTypeService(ISqlSugarClient client) : base(client)
        {
        }

        //如果没有特殊需求，接口里面什么都没定义那就无需在这里编码
        //如果有特殊需求，自定义接口的实现在下方完成...
    }
}