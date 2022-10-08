using Amz.EnumLib.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amz.EnumLib.Enums
{
    public enum DegreeEnum
    {
        [EnumDetail(Code = 1, Name = "学士")]
        BACHELOR,
        [EnumDetail(Code = 2, Name = "硕士")]
        MASTER,
        [EnumDetail(Code = 3, Name = "博士")]
        DOCTOR,
    }
}
