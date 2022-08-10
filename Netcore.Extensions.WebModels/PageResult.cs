namespace Netcore.Extensions.WebModels
{
    /// <summary>
    /// 基础分页数据结构
    /// </summary>
    /// <typeparam name="T">分页数据类型</typeparam>
    public class PageResult<T>
    {
        public PageResult(int pageIndex, int pageSize, int totalCount, List<T>? pageData)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            PageData = pageData;
        }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public List<T>? PageData { get; set; }
    }
}