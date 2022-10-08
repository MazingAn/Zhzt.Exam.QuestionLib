using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhzt.Exam.Auth.DomainDtoModel
{
    public class DepartmentDto
    {
        public long Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public List<DepartmentDto> Child { get; set; } = new List<DepartmentDto>();
    }
}
