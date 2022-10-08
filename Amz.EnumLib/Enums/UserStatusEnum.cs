using Amz.EnumLib.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amz.EnumLib.Enums
{
    public enum UserStatusEnum
    {
        [EnumDetail(Code = 0, Name = "正常")]
        Normal,
        [EnumDetail(Code = 1, Name = "锁定")]
        Lock,
        [EnumDetail(Code = 2, Name = "删除")]
        Deleted,
    }
}
