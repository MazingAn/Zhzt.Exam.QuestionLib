using AutoFilterer.Attributes;
using AutoFilterer.Enums;
using AutoFilterer.Types;
using System.Linq.Expressions;
using Zhzt.Exam.PaperLib.DomainModel;

namespace Zhzt.Exam.PaperLib.Api.Models
{
    public class DocPaperFilter : FilterBase
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public SubjectFilter? Subject { get; set; }

        [StringFilterOptions(StringFilterOption.Equals)]
        public string? Name { get; set; }
    }

    public class SubjectFilter : FilterBase
    {
        [StringFilterOptions(StringFilterOption.Equals)]
        public string? Name { get; set; }

        [StringFilterOptions(StringFilterOption.Equals)]
        public string? SubjectId { get; set; }
    }
}
