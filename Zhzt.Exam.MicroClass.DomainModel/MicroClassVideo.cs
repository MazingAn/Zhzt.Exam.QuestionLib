using SqlSugar;
using SqlSugar.Extension.DomainHelper;
using System.ComponentModel.DataAnnotations;

namespace Zhzt.Exam.MicroClass.DomainModel
{
    public class MicroClassVideo : BaseModel
    {
        [Required]
        [SugarColumn(ColumnDataType = "varchar(64)", IsNullable = false)]
        public string Title { get; set; } = null!;

        [SugarColumn(ColumnDataType = "varchar(256)")]
        public string Description { get; set; } = string.Empty;

        [SugarColumn(ColumnDataType = "varchar(256)")]
        public string Thumb { get; set; } = string.Empty;

        [SugarColumn(ColumnDataType = "varchar(256)")]
        public string VideoUrl { get; set; } = string.Empty;

        [SugarColumn(IsNullable = true)]
        public long VideoCategoryId { get; set; }

        public DateTime PubDate { get; set; } = DateTime.Now;

        [SugarColumn(IsIgnore = true)]
        public virtual VideoCategory? Category { get; set; }
    }
}