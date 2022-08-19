using MongoDb.Extensions.DomainHelper;
using MongoDB.Bson.Serialization.Attributes;

namespace Zhzt.Exam.PaperLib.DomainModel
{
    public class DocPaper : BaseModel
    {
        // 试卷名称
        public string Name { get; set; } = string.Empty;

        // 试卷作者信息
        public InnerDocUserInfo? Creator { get; set; }

        // 所属科目和包含科目的信息
        public InnerDocSubject Subject { get; set; } = null!;

        /// <summary>
        /// 试题配置信息
        /// </summary>
        public InnerDocPagerConfig PagerConfig { get; set; } = null!;

        // 单选题列表
        public IEnumerable<InnerDocPaperQuestion> SingleChoiceQuestions { get; set; } = null!;

        // 多选题列表
        public IEnumerable<InnerDocPaperQuestion> MultiChoiceQuestions { get; set; } = null!;

        //判断题列表
        public IEnumerable<InnerDocPaperQuestion> JudgeQuestions { get; set; } = null!;

        //填空题列表
        public IEnumerable<InnerDocPaperQuestion> BlankFillQuestions { get; set; } = null!;

        //问答题列表
        public IEnumerable<InnerDocPaperQuestion> QuesAnswerQuestions { get; set; } = null!;

        ///试卷路径
        public string? PaperFilePath { get; set; } = string.Empty;

        //是否重新生成试卷
        [BsonIgnore]
        public bool ReGenerateQuestions { get; set; } = false;
    }
}