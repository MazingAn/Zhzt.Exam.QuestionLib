using Microsoft.Extensions.Options;
using MongoDb.Extensions.Options;
using MongoDB.Driver;
using Netcore.Extensions.WebModels;
using System.Linq.Expressions;
using AutoFilterer.Extensions;
using AutoFilterer.Abstractions;
using AutoFilterer.Enums;
using AutoFilterer.Types;

namespace MongoDb.Extensions.DomainHelper
{
    public class BaseService<T> : IBaseService<T> where T : BaseModel
    {
        protected readonly IMongoCollection<T> _collection;

        public BaseService(
            IOptions<MongoDbDataSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(
                databaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                databaseSettings.Value.DatabaseName);

            _collection = mongoDatabase.GetCollection<T>(typeof(T).Name);
        }

        /// <summary>
        /// 添加一段数据
        /// </summary>
        /// <param name="t">数据实体</param>
        /// <returns>变更后的数据</returns>
        public T Create(T t)
        {
            _collection.InsertOne(t);
            return t;
        }

        /// <summary>
        /// 批量添加数据
        /// </summary>
        /// <param name="tList"></param>
        /// <returns></returns>
        public bool CreateMany(IEnumerable<T> tList)
        {
            _collection.InsertMany(tList);
            return true;
        }

        /// <summary>
        /// 筛选数据
        /// </summary>
        /// <param name="filter">筛选表达式</param>
        /// <returns>结果集合</returns>
        public IEnumerable<T> FilterAll(FilterBase filter)
        {
            filter.CombineWith = CombineType.And;
            return _collection.AsQueryable().ApplyFilter(filter).ToList();
        }

        /// <summary>
        /// 分页筛选数据
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        public PageResult<T> FilterPaged(FilterBase filter, int pageIndex, int pageSize)
        {
            filter.CombineWith = CombineType.And;
            var query = _collection.AsQueryable().ApplyFilter(filter);
            int totalCount = query.Count();
            var pageData = query.Take(pageSize).Skip((pageIndex - 1) * pageSize).ToList();
            return new PageResult<T>(pageIndex, pageSize, totalCount, pageData);
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<T> GetAll()
        {
            return _collection.Find(_ => true).ToList();
        }

        /// <summary>
        /// 根据ID获取一个
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetOneById(string id)
        {
            return _collection.Find(x => x.Id == id).First();
        }

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <returns></returns>
        public PageResult<T> GetPaged(int pageIndex, int pageSize)
        {
            var query = _collection.AsQueryable();
            int totalCount = query.Count();
            var pageData = query.Take(pageSize).Skip((pageIndex - 1) * pageSize).ToList();
            return new PageResult<T>(pageIndex, pageSize, totalCount, pageData);
        }

        /// <summary>
        /// 根据id删除数据
        /// </summary>
        /// <param name="id">id主键</param>
        /// <returns></returns>
        public bool Remove(string id)
        {
            _collection.DeleteOne(x=>x.Id == id);
            return true;
        }

        /// <summary>
        /// 根据id列表删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Remove(string[] ids)
        {
            _collection.DeleteMany(x => ids.Contains(x.Id));
            return true;
        }

        /// <summary>
        /// 按条件删除数据
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public bool Remove(FilterBase filter)
        {
            filter.CombineWith = CombineType.And;
            var deleteList = _collection.AsQueryable().ApplyFilter(filter).ToList();
            _collection.DeleteMany(x => deleteList.Select(a => a.Id).Contains(x.Id));
            return true;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public T Update(T t)
        {
            _collection.ReplaceOne(x => x.Id == t.Id, t);
            return t;
        }

        /// <summary>
        /// 查询总数
        /// </summary>
        public int Count()
        {
            return _collection.AsQueryable().Count();
        }

        /// <summary>
        /// 根据条件查询数量
        /// </summary>
        /// <param name="filterExpression"></param>
        public int Count(FilterBase filter)
        {
            filter.CombineWith = CombineType.And;
            return _collection.AsQueryable().ApplyFilter(filter).Count();
        }
    }
}
