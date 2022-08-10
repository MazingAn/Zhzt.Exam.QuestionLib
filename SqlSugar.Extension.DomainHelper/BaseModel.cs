using SqlSugar;

namespace SqlSugar.Extensions.DomainHelper
{
    /// <summary>
    /// 基础模型
    /// </summary>
    public class BaseModel
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public long Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsOnlyIgnoreUpdate = true)]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; } = DateTime.Now;
    }
}