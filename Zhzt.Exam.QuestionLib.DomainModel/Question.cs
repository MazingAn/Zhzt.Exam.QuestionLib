using SqlSugar;
using SqlsugarCodeFirst.QuickDomain;

namespace Zhzt.Exam.QuestionLib.DomainModel
{
    /// <summary>
    /// 题库的主题 题目表
    /// </summary>
    public class Question : BaseModel
    {
        /// <summary>
        /// 题干
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public string QuestionBody { get; set; } = string.Empty;

        /// <summary>
        /// 题目类型
        /// </summary>
        [SugarColumn(IsNullable = false, IndexGroupNameList = new string[] { "QuestionTypeId" })]
        public long QuestionTypeId { get; set; }

        /// <summary>
        /// 题目类别 单选 多选 判断 填空 问答
        /// </summary>
        [SugarColumn(IsNullable = false, IndexGroupNameList = new string[] { "QuestionClass" })]
        public int QuestionClass { get; set; }

        /// <summary>
        /// 选项1
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Answer1 { get; set; } = string.Empty;

        /// <summary>
        /// 选项2
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Answer2 { get; set; } = string.Empty;

        /// <summary>
        /// 选项3
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Answer3 { get; set; } = string.Empty;

        /// <summary>
        /// 选项4
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Answer4 { get; set; } = string.Empty;

        /// <summary>
        /// 选项5
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Answer5 { get; set; } = string.Empty;

        /// <summary>
        /// 选项6
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Answer6 { get; set; } = string.Empty;

        /// <summary>
        /// 正确答案
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public string RightAnswer { get; set; } = string.Empty;
    }
}
