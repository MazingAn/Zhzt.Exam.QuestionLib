using Netcore.Extensions.WebModels;
using System.Linq.Expressions;

namespace SqlSugar.Extension.DomainHelper
{
    /// <summary>
    /// 给予sqlsugar的基础增删改查方法
    /// </summary>
    public interface IBaseService
    {
        /// <summary>
        /// 保存对象
        /// </summary>
        /// <typeparam name="T">对象模型类型</typeparam>
        /// <param name="t">对象实例</param>
        /// <returns>保存后的对象</returns>
        T? Save<T>(T t) where T : BaseModel, new();

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <typeparam name="T">对象模型类型</typeparam>
        /// <param name="t">对象实体</param>
        /// <returns>变更后的对象</returns>
        T? Update<T>(T t) where T : BaseModel, new();

        /// <summary>
        /// 根据ID删除对象
        /// </summary>
        /// <typeparam name="T">对象模型类型</typeparam>
        /// <param name="id">对象id</param>
        /// <returns>是否删除成功</returns>
        bool Delete<T>(long id) where T : BaseModel, new();

        /// <summary>
        /// 根据Id数组批量删除数据【事务级别】
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="ids">主键数组</param>
        /// <returns>是否删除成功</returns>
        bool Delete<T>(IList<long> ids) where T : BaseModel, new();

        /// <summary>
        /// 根据表达式批量删除
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="predicate">过滤表达式</param>
        /// <returns>是否删除成功</returns>
        int Delete<T>(Expression<Func<T, bool>> predicate) where T : BaseModel, new();

        /// <summary>
        /// 根据ID查找对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="id">对象id</param>
        /// <returns>查询到的结果</returns>
        T? GetById<T>(long id) where T : BaseModel;

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <returns>数据集合的可迭代对象</returns>
        IEnumerable<T> GetAll<T>() where T : BaseModel;

        /// <summary>
        /// 分页查询和排序
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="pageIndex">分页页码</param>
        /// <param name="pageSize">页码尺寸</param>
        /// <param name="orderExpression">排序字段</param>
        /// <returns>SqlSugar内置的分页模型</returns>
        PageResult<T> GetPage<T>(int pageIndex, int pageSize, Expression<Func<T, object>> orderExpression) where T : BaseModel;

        /// <summary>
        /// 按条件筛选数据
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="filterExpression">筛选表达式</param>
        /// <returns>符合条件的数据集合的可迭代对象</returns>
        IEnumerable<T> Filter<T>(Expression<Func<T, bool>> filterExpression) where T : BaseModel;

        /// <summary>
        /// 按照条件筛选数据并分页
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="pageIndex">分页页码</param>
        /// <param name="pageSize">分页尺寸</param>
        /// <param name="filterExpression">过滤条件</param>
        /// <returns>符合条件的数据集合的分页模型</returns>
        PageResult<T> FilterPage<T>(int pageIndex, int pageSize, Expression<Func<T, bool>> filterExpression) where T : BaseModel;

        /// <summary>
        /// 对对象进行计数
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>记录数量</returns>
        int Count<T>() where T : BaseModel;

        /// <summary>
        /// 对符合筛选条件的对象进行计数
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="filterExpression">过滤表达式</param>
        /// <returns>数量</returns>
        int Count<T>(Expression<Func<T, bool>> filterExpression) where T : BaseModel;

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
        PrimaryT SaveMapper<PrimaryT, ChileT, MapperT>(PrimaryT primary, List<ChileT>? chiles)
            where PrimaryT : BaseModel, new()
            where ChileT : BaseModel, new()
            where MapperT : MapperModel, new();

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
            where MapperT : MapperModel, new();

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
            where MapperT : MapperModel, new();

        /// <summary>
        /// 根据ID 多对多删除
        /// </summary>
        /// <typeparam name="PrimaryT">主表类型</typeparam>
        /// <typeparam name="MapperT">子表类型</typeparam>
        /// <param name="id">主表ID</param>
        /// <returns>删除成功返回True 失败返回Fales</returns>
        bool DeleteMapper<PrimaryT, MapperT>(long id)
           where PrimaryT : BaseModel, new()
           where MapperT : MapperModel, new();

        /// <summary>
        /// 更新数据 多对多更新
        /// </summary>
        /// <typeparam name="PrimaryT">主表类型</typeparam>
        /// <typeparam name="ChileT">子表类型</typeparam>
        /// <typeparam name="MapperT">中间表类型</typeparam>
        /// <param name="primaryT">主表对象</param>
        /// <param name="chileTs">主表中的子表集合 例如：primaryT.chiles</param>
        /// <returns>更新成功返回更新后的数据</returns>
        PrimaryT UpdateMapper<PrimaryT, ChileT, MapperT>(PrimaryT primaryT, List<ChileT>? chileTs)
           where PrimaryT : BaseModel, new()
           where ChileT : BaseModel, new()
           where MapperT : MapperModel, new();

        /// <summary>
        /// 分页查询 多对多分页查询
        /// </summary>
        /// <typeparam name="PrimaryT">主表类型</typeparam>
        /// <typeparam name="ChileT">子表类型</typeparam>
        /// <typeparam name="MapperT">中间表类型</typeparam>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页数量</param>
        /// <param name="orderExpression">排序表达式 根据什么排序</param>
        /// <returns>分页数据</returns>
        PageResult<PrimaryT> GetPageMapper<PrimaryT, ChileT, MapperT>(Expression<Func<PrimaryT, object>> orderExpression, int pageIndex = 1, int pageSize = 10)
           where PrimaryT : BaseModel, new()
           where ChileT : BaseModel, new()
           where MapperT : MapperModel, new();

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
            where MapperT : MapperModel, new();

        #endregion 一对多 多对多查询

        #region 子级查询

        /// <summary>
        /// 查询所有Tree数据
        /// </summary>
        /// <typeparam name="TreeT"></typeparam>
        /// <returns></returns>
        IEnumerable<TreeT> GetAllTree<TreeT>()
            where TreeT : TreeModel<TreeT>, new();

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PageResult<TreeT> GetPageTree<TreeT>(int pageIndex = 1, int pageSize = 10)
            where TreeT : TreeModel<TreeT>, new();

        /// <summary>
        /// 根据id删除数据（以及子数据）
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        bool? DeleteByIdTree<TreeT>(long id)
            where TreeT : TreeModel<TreeT>, new();

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TreeT? GetByIdTree<TreeT>(long id)
            where TreeT : TreeModel<TreeT>, new();

        IEnumerable<TreeT> GetAllChildren<TreeT>(long id)
            where TreeT : TreeModel<TreeT>, new();

        #endregion 子级查询
    }
}