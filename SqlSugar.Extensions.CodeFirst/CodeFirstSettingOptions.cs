using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugar.Extensions.CodeFirst
{
    /// <summary>
    /// CodeFirst配置选项
    /// </summary>
    public class CodeFirstSettingOptions
    {
        public const string CodeFirstSettings = "CodeFirstSettings";

        public bool Migrate { get; set; }

        public bool Backup { get; set; }

        public string ModelPath { get; set; } = String.Empty;
    }
}
