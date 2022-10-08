using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amz.EnumLib.Attributes
{
    public class EnumDetailAttribute : Attribute
    {
        public string Name { get; set; } = string.Empty;

        public int Code { get; set; }
    }
}
