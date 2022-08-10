using Netcore.Extensions.WebModels;
using System.Linq.Expressions;

namespace SqlSugar.Extensions.DomainHelper
{
    /// <summary>
    /// 基于SqlSugar的基础增删改查接口的实现，这种实现默认支持二级缓存
    /// </summary>
    public class BaseCachedService : BaseService, IBaseService
    {
        public BaseCachedService(ISqlSugarClient client) : base(client)
        {
        }

        /// <summary>
        /// 根据ID删除
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="id">id</param>
        /// <returns>删除是否成功</returns>
        public new bool Delete<T>(long id) where T : BaseModel, new()
        {
            return _client?.Deleteable<T>().RemoveDataCache().In(id).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 根据id数组删除
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="ids">id数组</param>
        /// <returns>是否删除成功</returns>
        public new bool Delete<T>(IList<long> ids) where T : BaseModel, new()
        {
            return _client?.Deleteable<T>().RemoveDataCache().In(ids).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 根据筛选条件删除
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="predicate">删除条件</param>
        /// <returns>删除的行数 -1表示删除失败</returns>
        public new int Delete<T>(Expression<Func<T, bool>> predicate) where T : BaseModel, new()
        {
            return _client?.Deleteable<T>().RemoveDataCache().Where(predicate).ExecuteCommand() ?? -1;
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <returns>数据集合</returns>
        public new IEnumerable<T> GetAll<T>() where T : BaseModel
        {
            return _client?.Queryable<T>().WithCache().ToList() ?? new List<T>();
        }

        /// <summary>
        /// 根据id查找对象
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <param name="id">给定的ID主键</param>
        /// <returns>查询结果 可为null</returns>
        public new T? GetById<T>(long id) where T : BaseModel
        {
            return _client?.Queryable<T>().WithCache().InSingle(id);
        }

        /// <summary>
        /// 添加数据 自动设置雪花ID
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="t">对象实例</param>
        /// <returns>添加后的数据</returns>
        public new T? Save<T>(T t) where T : BaseModel, new()
        {
            t.Id = SnowFlakeSingle.Instance.getID();
            return _client?.Insertable<T>(t).RemoveDataCache().ExecuteReturnEntity();
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <typeparam name="T">修改的对象类型</typeparam>
        /// <param name="t">修改的对象</param>
        /// <returns>修改后的数据</returns>
        public new T? Update<T>(T t) where T : BaseModel, new()
        {
            _client?.Updateable<T>(t).RemoveDataCache().ExecuteCommand();
            return t;
        }
    }
}