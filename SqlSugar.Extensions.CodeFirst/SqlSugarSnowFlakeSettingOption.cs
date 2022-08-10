using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugar.Extensions.CodeFirst
{
    public class SqlSugarSnowFlakeSettingOption
    {
        public const string SqlSugarSnowFlakeSettings = "SqlSugarSnowFlakeSettings";

        public int WorkerId { get; set; } = 1;
    }
}
