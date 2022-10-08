using SqlSugar;
using SqlSugar.Extension.DomainHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhzt.Exam.MicroClass.DomainModel
{
    public class VideoCategory : TreeModel<VideoCategory>
    {
        [Required]
        [SugarColumn(IsNullable = false, ColumnDataType = "varchar(64)")]
        public string Name { get; set; } = null!;
    }
}
