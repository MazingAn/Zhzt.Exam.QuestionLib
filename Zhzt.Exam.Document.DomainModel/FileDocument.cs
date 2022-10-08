using SqlSugar;
using SqlSugar.Extension.DomainHelper;
using System.ComponentModel.DataAnnotations;

namespace Zhzt.Exam.Document.DomainModel
{
    public class FileDocument : BaseModel
    {
        [Required]
        [SugarColumn(ColumnDataType = "varchar(64)")]
        public string Name { get; set; } = null!;

        [SugarColumn(ColumnDataType = "varchar(128)")]
        public string DocUrl { get; set; } = string.Empty;

        [SugarColumn(ColumnDataType = "varchar(128)")]
        public string PdfUrl { get; set; } = string.Empty;

        [SugarColumn(ColumnDataType = "varchar(128)")]
        public string Thumb { get; set; } = string.Empty;

        public long CategoryId { get; set; }

        [SugarColumn(IsIgnore = true)]
        public DocumentCategory? DocCategory { get; set; }
    }
}