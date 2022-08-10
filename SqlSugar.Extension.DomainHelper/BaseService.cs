using Netcore.Extensions.WebModels;
using System.Linq.Expressions;

namespace SqlSugar.Extensions.DomainHelper
{
    /// <summary>
    /// 基于SqlSugar的基础增删改查接口的实现
    /// </summary>
    public class BaseService : IBaseService
    {
        /// <summary>
        /// 需要外部注入的_client
        /// </summary>
        protected readonly ISqlSugarClient? _client;

        /// <summary>
        /// 构造函数 注入入口
        /// </summary>
        /// <param name="client">ISqlSugarClient实例</param>
        public BaseService(ISqlSugarClient client)
        {
            _client = client;
        }

        /// <summary>
        /// 总体计数
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>结束结果，异常返回-1</returns>
        public int Count<T>() where T : BaseModel
        {
            return _client?.Queryable<T>().Count() ?? -1;
        }

        /// <summary>
        /// 按条件计数
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="filterExpression">筛选条件</param>
        /// <returns>计数数量，异常返回 -1</returns>
        public int Count<T>(Expression<Func<T, bool>> filterExpression) where T : BaseModel
        {
            return _client?.Queryable<T>().Where(filterExpression).Count() ?? -1;
        }

        /// <summary>
        /// 根据ID删除
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="id">id</param>
        /// <returns>删除是否成功</returns>
        public bool Delete<T>(long id) where T : BaseModel, new()
        {
            return _client?.Deleteable<T>().In(id).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 根据id数组删除
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="ids">id数组</param>
        /// <returns>是否删除成功</returns>
        public bool Delete<T>(IList<long> ids) where T : BaseModel, new()
        {
            return _client?.Deleteable<T>().In(ids).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 根据筛选条件删除
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="predicate">删除条件</param>
        /// <returns>删除的行数 -1表示删除失败</returns>
        public int Delete<T>(Expression<Func<T, bool>> predicate) where T : BaseModel, new()
        {
            return _client?.Deleteable<T>().Where(predicate).ExecuteCommand() ?? -1;
        }

        /// <summary>
        /// 根据条件筛选
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="filterExpression">筛选条件</param>
        /// <returns>筛选结果可迭代对象</returns>
        public IEnumerable<T> Filter<T>(Expression<Func<T, bool>> filterExpression) where T : BaseModel
        {
            return _client?.Queryable<T>().Where(filterExpression).ToList() ?? new List<T>();
        }

        /// <summary>
        ///  分页筛选数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页码尺寸</param>
        /// <param name="filterExpression">筛选表达式</param>
        /// <returns>分页数据</returns>
        public PageResult<T> FilterPage<T>(int pageIndex, int pageSize, Expression<Func<T, bool>> filterExpression) where T : BaseModel
        {
            int totalCount = 0;
            var page = _client?.Queryable<T>().Where(filterExpression).ToPageList(pageIndex, pageSize, ref totalCount);
            return new PageResult<T>(pageIndex, pageSize, totalCount, page);
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <returns>数据集合</returns>
        public IEnumerable<T> GetAll<T>() where T : BaseModel
        {
            return _client?.Queryable<T>().ToList() ?? new List<T>();
        }

        /// <summary>
        /// 根据id查找对象
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <param name="id">给定的ID主键</param>
        /// <returns>查询结果 可为null</returns>
        public T? GetById<T>(long id) where T : BaseModel
        {
            return _client?.Queryable<T>().InSingle(id);
        }

        /// <summary>
        /// 按顺序分页查询
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页尺寸</param>
        /// <param name="orderExpression">排序表达式</param>
        /// <returns>分页结果</returns>
        public PageResult<T> GetPage<T>(int pageIndex, int pageSize, Expression<Func<T, object>> orderExpression) where T : BaseModel
        {
            int totalCount = 0;
            var page = _client?.Queryable<T>().OrderBy(orderExpression).ToPageList(pageIndex, pageSize, ref totalCount);
            return new PageResult<T>(pageIndex, pageSize, totalCount, page);
        }

        /// <summary>
        /// 添加数据 自动设置雪花ID
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="t">对象实例</param>
        /// <returns>添加后的数据</returns>
        public T? Save<T>(T t) where T : BaseModel, new()
        {
            t.Id = SnowFlakeSingle.Instance.getID();
            return _client?.Insertable<T>(t).ExecuteReturnEntity();
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <typeparam name="T">修改的对象类型</typeparam>
        /// <param name="t">修改的对象</param>
        /// <returns>修改后的数据</returns>
        public T? Update<T>(T t) where T : BaseModel, new()
        {
            _client?.Updateable<T>(t).ExecuteCommand();
            return t;
        }

        #region 一对多 多对多查询

        /// <summary>
        /// 保存 多对多
        /// </summary>
        /// <typeparam name="PrimaryT">主表类型</typeparam>
        /// <typeparam name="ChileT">子表类型</typeparam>
        /// <typeparam name="MapperT">映射表类型</typeparam>
        /// <param name="primary">主表对象</param>
        /// <param name="chiles">主表中的子表集合 例如：primaryT.chiles</param>
        /// <returns>返回保存成功后的数据</returns>
        public PrimaryT SaveMapper<PrimaryT, ChileT, MapperT>(PrimaryT primary, List<ChileT>? chiles)
            where PrimaryT : BaseModel, new()
            where ChileT : BaseModel, new()
            where MapperT : MapperModel, new()
        {
            _client?.Ado.BeginTran();
            try
            {
                primary.Id = SnowFlakeSingle.Instance.getID();
                var savePrimary = Save<PrimaryT>(primary);
                if (chiles is not null)
                {
                    foreach (var chile in chiles)
                    {
                        Save<MapperT>(new MapperT()
                        {
                            Id = SnowFlakeSingle.Instance.getID(),
                            PrimaryTableId = savePrimary is not null ? savePrimary.Id : 0,
                            ChileTableId = chile.Id,
                        });
                    }
                }
                _client?.Ado.CommitTran();
                return savePrimary ?? throw new Exception("创建失败");
            }
            catch (Exception ex)
            {
                _client?.Ado.RollbackTran();
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据ID 多对多查询
        /// </summary>
        /// <typeparam name="PrimaryT">主表类型</typeparam>
        /// <typeparam name="ChileT">子表类型</typeparam>
        /// <param name="id">主表ID 需查询的主表对象ID</param>
        /// <returns>返回查询到的数据</returns>
        public PrimaryT GetByIdMapper<PrimaryT, ChileT, MapperT>(long id)
           where PrimaryT : BaseModel, new()
           where ChileT : BaseModel, new()
            where MapperT : MapperModel, new()
        {
            //var data = include1 is not null ? _client?.Queryable<PrimaryT>().Includes(include1).InSingle(id) : throw new Exception("include1 表达式不能null");
            //return data ?? throw new Exception("查询失败");
            var data = _client?.Queryable<PrimaryT>()
                .Mapper<PrimaryT, ChileT, MapperT>(mapper => ManyToMany.Config(mapper.PrimaryTableId, mapper.ChileTableId))
                .InSingle(id);
            return data ?? throw new Exception("查询失败");
        }

        /// <summary>
        /// 查询所有 多对多查询
        /// </summary>
        /// <typeparam name="PrimaryT">主表类型</typeparam>
        /// <typeparam name="ChileT">子表类型</typeparam>
        /// <typeparam name="MapperT">中间表类型</typeparam>
        /// <returns>返回所有数据</returns>
        /// <exception cref="Exception">查询失败抛出异常</exception>
        public IEnumerable<PrimaryT> GetAllMapper<PrimaryT, ChileT, MapperT>()
            where PrimaryT : BaseModel, new()
            where ChileT : BaseModel, new()
            where MapperT : MapperModel, new()
        {
            IEnumerable<PrimaryT>? primaryTs = _client?.Queryable<PrimaryT>()
                .Mapper<PrimaryT, ChileT, MapperT>(
                    mapper => ManyToMany.Config(mapper.PrimaryTableId, mapper.ChileTableId))
                .ToList();
            return primaryTs ?? throw new Exception("查询失败");
        }

        /// <summary>
        /// 根据ID 多对多删除
        /// </summary>
        /// <typeparam name="PrimaryT">主表类型</typeparam>
        /// <typeparam name="MapperT">子表类型</typeparam>
        /// <param name="id">主表ID</param>
        /// <returns>删除成功返回True 失败返回Fales</returns>
        public bool DeleteMapper<PrimaryT, MapperT>(long id)
           where PrimaryT : BaseModel, new()
           where MapperT : MapperModel, new()
        {
            _client?.Ado.BeginTran();
            try
            {
                Delete<PrimaryT>(id);
                Delete<MapperT>(lcl => lcl.PrimaryTableId == id);
                _client?.Ado.CommitTran();
                return true;
            }
            catch
            {
                _client?.Ado.RollbackTran();
                throw new Exception("删除失败");
            }
        }

        /// <summary>
        /// 更新数据 多对多更新
        /// </summary>
        /// <typeparam name="PrimaryT">主表类型</typeparam>
        /// <typeparam name="ChileT">子表类型</typeparam>
        /// <typeparam name="MapperT">中间表类型</typeparam>
        /// <param name="primaryT">主表对象</param>
        /// <param name="chileTs">主表中的子表集合 例如：primaryT.chiles</param>
        /// <returns>更新成功返回更新后的数据</returns>
        public PrimaryT UpdateMapper<PrimaryT, ChileT, MapperT>(PrimaryT primaryT, List<ChileT>? chileTs)
            where PrimaryT : BaseModel, new()
            where ChileT : BaseModel, new()
            where MapperT : MapperModel, new()
        {
            _client?.Ado.BeginTran();
            try
            {
                var data = Update(primaryT);
                Delete<MapperT>(p => p.PrimaryTableId == primaryT.Id);
                if (data is not null && chileTs is not null)
                    foreach (var item in chileTs)
                    {
                        Save(new MapperT()
                        {
                            Id = SnowFlakeSingle.Instance.getID(),
                            PrimaryTableId = primaryT.Id,
                            ChileTableId = item.Id
                        });
                    }
                _client?.Ado.CommitTran();
                return data ?? throw new Exception("更新失败");
            }
            catch
            {
                _client?.Ado.RollbackTran();
                throw new Exception("更新失败");
            }
        }

        /// <summary>
        /// 分页查询 多对多分页查询
        /// </summary>
        /// <typeparam name="PrimaryT">主表类型</typeparam>
        /// <typeparam name="ChileT">子表类型</typeparam>
        /// <typeparam name="MapperT">中间表类型</typeparam>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="orderExpression">排序表达式 根据什么排序</param>
        /// <returns>分页数据</returns>
        public PageResult<PrimaryT> GetPageMapper<PrimaryT, ChileT, MapperT>(Expression<Func<PrimaryT, object>> orderExpression, int pageIndex = 1, int pageSize = 10)
            where PrimaryT : BaseModel, new()
            where ChileT : BaseModel, new()
            where MapperT : MapperModel, new()
        {
            int totalCount = 0;
            var page = _client?.Queryable<PrimaryT>()
                .OrderBy(orderExpression)
                .Mapper<PrimaryT, ChileT, MapperT>(
                    mapper => ManyToMany.Config(mapper.PrimaryTableId, mapper.ChileTableId))
                .ToPageList(pageIndex, pageSize, ref totalCount);
            return new PageResult<PrimaryT>(pageIndex, pageSize, totalCount, page);
        }

        /// <summary>
        /// 根据时间进行过滤 多对多过滤
        /// </summary>
        /// <typeparam name="PrimaryT">主表类型</typeparam>
        /// <typeparam name="ChileT">子表类型</typeparam>
        /// <typeparam name="MapperT">中间表类型</typeparam>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        public PageResult<PrimaryT> GetDataByTimeSpanMapper<PrimaryT, ChileT, MapperT>(DateTime startTime, DateTime endTime, int pageIndex = 1, int pageSize = 10)
            where PrimaryT : BaseModel, new()
            where ChileT : BaseModel, new()
            where MapperT : MapperModel, new()
        {
            int totalCount = 0;
            var page = _client?.Queryable<PrimaryT>()
                .Where(primaryT => primaryT.CreateTime > startTime && primaryT.CreateTime < endTime)
                .Mapper<PrimaryT, ChileT, MapperT>(
                    mapper => ManyToMany.Config(mapper.PrimaryTableId, mapper.ChileTableId))
                .ToPageList(pageIndex, pageSize, ref totalCount);
            return new PageResult<PrimaryT>(pageIndex, pageSize, totalCount, page);
        }

        #endregion 一对多 多对多查询

        #region 子级查询

        /// <summary>
        /// 查询所有Tree数据
        /// </summary>
        /// <typeparam name="TreeT"></typeparam>
        /// <returns></returns>
        public IEnumerable<TreeT> GetAllTree<TreeT>()
            where TreeT : TreeModel<TreeT>, new()
        {
            //IEnumerable<TreeT>? page = _client?.Queryable<TreeT>()
            //.Mapper(d => d.Child, d => d.Child.First().Id, d => d.ParentId)
            //.ToList();

            IEnumerable<TreeT>? trees = _client?.Queryable<TreeT>()
                .ToTree(it => it.Child, it => it.ParentId, 0);
            return trees ?? new List<TreeT>();
        }

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PageResult<ChildrenT> GetPageTree<ChildrenT>(int pageIndex = 1, int pageSize = 10)
            where ChildrenT : TreeModel<ChildrenT>, new()
        {
            int totalCount = 0;
            var page = _client?.Queryable<ChildrenT>()
                .ToTree(d => d.Child, d => d.ParentId,0)
                .Take(pageSize).Skip((pageIndex-1) * pageSize)
                .ToList();
            totalCount = _client?.Queryable<ChildrenT>().Where(t => t.ParentId == 0).Count() ?? 0;
            return new PageResult<ChildrenT>(pageIndex, pageSize, totalCount, page);
        }

        /// <summary>
        /// 根据id删除数据（以及子数据）
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool? DeleteByIdTree<ChildrenT>(long id)
            where ChildrenT : TreeModel<ChildrenT>, new()
        {
            IEnumerable<ChildrenT> children = _client?.Queryable<ChildrenT>().ToChildList(q => q.ParentId, id) ?? new();
            _client?.Ado.BeginTran();
            try
            {
                Delete<ChildrenT>(id);
                foreach (var item in children)
                {
                    Delete<ChildrenT>(item.Id);
                }
                _client?.Ado.CommitTran();
                return true;
            }
            catch
            {
                _client?.Ado.RollbackTran();
                return false;
            }
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ChildrenT? GetByIdTree<ChildrenT>(long id)
            where ChildrenT : TreeModel<ChildrenT>, new()
        {
            var rootValue = _client?.Queryable<ChildrenT>().InSingle(id);
            //var tree = _client?.Queryable<ChildrenT>().ToChildList(it => it.ParentId, id); //返回结构不一样 list结构
            return _client?.Queryable<ChildrenT>()
                .ToTree(it => it.Child, it => it.ParentId, rootValue?.ParentId)
                .Single(it => it.Id == id);
        }

        #endregion 子级查询
    }
}