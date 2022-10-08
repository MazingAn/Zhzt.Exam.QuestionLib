using Amz.EnumLib.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Amz.EnumLib.Enums
{
    public enum EducationEnum
    {
        [EnumDetail(Code = 1, Name = "小学")]
        PRIMARY_SCHOOL,
        [EnumDetail(Code = 2, Name = "中学")]
        JUNIOR_MIDDLE_SCHOOL,
        [EnumDetail(Code = 3, Name = "高中")]
        HIGH_SCHOOL,
        [EnumDetail(Code = 4, Name = "大学")]
        UNIVERSITY,
        [EnumDetail(Code = 5, Name = "研究生")]
        POSTGRADUATE,
        [EnumDetail(Code = 6, Name = "博士")]
        DOCTOR,
    }
}
