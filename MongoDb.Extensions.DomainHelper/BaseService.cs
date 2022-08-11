using Microsoft.Extensions.Options;
using MongoDb.Extensions.Options;
using MongoDB.Driver;
using Netcore.Extensions.WebModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MongoDb.Extensions.DomainHelper
{
    public class BaseService<T> : IBaseService<T> where T : BaseModel
    {
        private readonly IMongoCollection<T> _collection;

        public BaseService(
            IOptions<MongoDbDataSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(
                databaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                databaseSettings.Value.DatabaseName);

            _collection = mongoDatabase.GetCollection<T>(nameof(T));
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
        /// <param name="filterExpression">筛选表达式</param>
        /// <returns>结果集合</returns>
        public IEnumerable<T> FilterAll(Expression<Func<T, bool>> filterExpression)
        {
            return _collection.AsQueryable().Where(filterExpression).ToList();
        }

        /// <summary>
        /// 分页筛选数据
        /// </summary>
        /// <param name="filterExpression">筛选条件</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        public PageResult<T> FilterPaged(Expression<Func<T, bool>> filterExpression, int pageIndex, int pageSize)
        {
            var query = _collection.AsQueryable().Where(filterExpression);
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
        /// <param name="filterExpression"></param>
        /// <returns></returns>
        public bool Remove(Expression<Func<T, bool>> filterExpression)
        {
            _collection.DeleteMany(filterExpression);
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
    }
}
