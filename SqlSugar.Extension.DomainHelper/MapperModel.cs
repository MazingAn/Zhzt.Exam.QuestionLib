using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugar.Extensions.DomainHelper
{
    /// <summary>
    /// 基础映射类
    /// </summary>
    public class MapperModel : BaseModel
    {
        /// <summary>
        /// 主表ID
        /// </summary>
        //[SugarColumn(IndexGroupNameList = new string[] { "PrimaryTableId", "ChileTableId" })]
        public long PrimaryTableId { get; set; }

        /// <summary>
        /// 子表ID
        /// </summary>
        //[SugarColumn(IndexGroupNameList = new string[] { "PrimaryTableId", "ChileTableId" })]
        public long ChileTableId { get; set; }
    }
}