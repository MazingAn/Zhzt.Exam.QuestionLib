using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amz.NPOIWord.Extension
{
    public class CustomerNPOITableData
    {
        public List<string> Headers { get; private set; } = new ();
        public List<List<string>> Rows { get; private set; } = new();


        /// <summary>
        /// 设置标题
        /// </summary>
        /// <param name="headers">表头的字符串列表</param>
        public void SetHeaders(List<string> headers)
        {
            this.Headers = headers;
        }

        /// <summary>
        /// 增加一列数据
        /// </summary>
        /// <param name="header">表头名称</param>
        /// <param name="colDatas">列数据</param>
        public void AppendColumn(string header, List<string> colDatas)
        {
            Headers.Add(header);
            for (int i = 0; i < colDatas.Count; i++)
            {
                Rows[i].Append(colDatas[i]);
            }
        }

        /// <summary>
        /// 增加一行数据
        /// </summary>
        /// <param name="row">行数据</param>
        /// <returns></returns>
        public void AppendRow(List<string> row)
        {
            Rows.Add(row);
        }

        /// <summary>
        /// 增加批量行
        /// </summary>
        /// <param name="appendedRows">多行数据</param>
        public void AppendRows(List<List<string>> appendedRows)
        {
            this.Rows = Rows.Concat(appendedRows).ToList();
        }

        /// <summary>
        /// 设置行数据
        /// </summary>
        /// <param name="rows">行数据</param>
        public void SetRows(List<List<string>> rows)
        {
            this.Rows = rows;
        }
    }
}
