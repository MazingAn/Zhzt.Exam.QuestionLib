using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugar.Extensions.DomainHelper
{
    public class TreeModel<TreeT> : BaseModel
    {
        /// <summary>
        /// 上级单位ID
        /// </summary>
        public long ParentId { get; set; } = 0;

        /// <summary>
        /// 下级单位列表
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<TreeT> Child { get; set; } = new List<TreeT>();
    }
}