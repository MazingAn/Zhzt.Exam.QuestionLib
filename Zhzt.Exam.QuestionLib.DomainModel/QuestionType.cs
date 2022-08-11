using SqlSugar;
using SqlSugar.Extension.DomainHelper;

namespace Zhzt.Exam.QuestionLib.DomainModel
{
    /// <summary>
    /// 问题所述的类别 【可以是专业类别，也可以是课目类别，可以嵌套】
    /// 比如，可以有如下数据库结构
    /// ------------------------------------------------------
    /// |      ID        |     Name       |    ParentId      |
    /// |      01        |     语文        |                  |
    /// |      11        |     古诗词      |      01          |  
    /// |      12        |     文言文      |      01          |
    /// |      13        |     拼音       |       01          |
    /// ------------------------------------------------------
    /// 上表中的语文就包含了古诗词、文言文、拼音
    /// 这样就可以保证在抽取试题的时候进行处理：
    /// 如果选择了一个大科目 比如语文，题库的范围对应古诗词、文言文、拼音
    /// 如果选择的是一个小科目 比如拼音 题库的范围则只会对应拼音 以及拼音的子类别
    /// </summary>
    public class QuestionType : TreeModel<QuestionType>
    {
        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(ColumnDataType = "varchar(64)", IsNullable = false)]
        public string Name { get; set; } = String.Empty;
    }
}
