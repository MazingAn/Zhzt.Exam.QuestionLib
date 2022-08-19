using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhzt.Exam.PaperLib.DomainModel
{
    public class InnerDocSubject
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 自己的ID-对应mysql数据库
        /// </summary>
        public string SubjectId { get; set; } = string.Empty;

        /// <summary>
        /// 上级ID-对应mysql数据库
        /// </summary>
        public string ParentId { get; set; } = string.Empty;

        /// <summary>
        /// 下级科目
        /// </summary>
        public IEnumerable<InnerDocSubject>? Children { get; set; }


    }
}
