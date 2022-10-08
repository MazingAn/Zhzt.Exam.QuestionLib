using Amz.EnumLib.Attributes;

namespace Amz.EnumLib.Enums
{
    public enum GenderEnum
    {
        [EnumDetail(Code = 0, Name = "未知")]
        Unknow,
        [EnumDetail(Code = 1, Name = "男")]
        Male,
        [EnumDetail(Code = 2, Name = "女")]
        FeMale,
        [EnumDetail(Code = 3, Name = "保密")]
        Secret,
    }
}