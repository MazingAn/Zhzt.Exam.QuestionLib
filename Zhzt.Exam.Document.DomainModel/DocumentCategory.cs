using SqlSugar;
using SqlSugar.Extension.DomainHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhzt.Exam.Document.DomainModel
{
    public class DocumentCategory : TreeModel<DocumentCategory>
    {
        [Required]
        [SugarColumn(ColumnDataType = "varchar(64)")]
        public string Name { get; set; } = null!;

        [SugarColumn(ColumnDataType = "varchar(256)")]
        public string Description { get; set; } = string.Empty;
    }
}
