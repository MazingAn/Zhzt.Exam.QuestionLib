using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhzt.Exam.PaperLib.Configuration
{
    public class PaperGenerateSettings
    {
        /// <summary>
        /// 默认的存储路径
        /// </summary>
        public string BaseDir { get; set; } = string.Empty;

        /// <summary>
        /// 默认的标题字体
        /// </summary>
        public string DefaultHeaderFontName { get; set; } = "黑体";

        /// <summary>
        /// 默认的字体
        /// </summary>
        public string DefaultFontName { get; set; } = "宋体";

        /// <summary>
        /// 默认的纸张规格
        /// </summary>
        public string DefaultPaperSize { get; set; } = "A3";

        /// <summary>
        /// 横板布局
        /// </summary>
        public bool LandSpace { get; set; } = false;

        /// <summary>
        /// 默认字体大小
        /// </summary>
        public int DefaultFontSize { get; set; } = 11;

        /// <summary>
        /// 是否附加答案
        /// </summary>
        public bool AttachAnswers = true;
    }
}
