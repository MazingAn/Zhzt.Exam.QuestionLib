
using AutoFilterer.Types;
using Netcore.Extensions.WebModels;
using System.Linq.Expressions;

namespace MongoDb.Extensions.DomainHelper
{
    public interface IBaseService<T> where T : BaseModel
    {
        public IEnumerable<T> GetAll();

        public T GetOneById(string id);

        public IEnumerable<T> FilterAll(FilterBase filter);

        public PageResult<T> GetPaged(int pageIndex, int pageSize);

        public PageResult<T> FilterPaged(FilterBase filter, int pageIndex, int pageSize);

        public T Create(T t);

        public bool CreateMany(IEnumerable<T> tList);

        public T Update(T t);

        public bool Remove(string id);

        public bool Remove(string[] ids);

        public bool Remove(FilterBase filter);

        public int Count();

        public int Count(FilterBase filter);

    }
}
