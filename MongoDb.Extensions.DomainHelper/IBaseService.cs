using Netcore.Extensions.WebModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MongoDb.Extensions.DomainHelper
{
    public interface IBaseService<T> where T : BaseModel
    {
        public IEnumerable<T> GetAll();

        public IEnumerable<T> FilterAll(Expression<Func<T, bool>> filterExpression);

        public PageResult<T> GetPaged(int pageIndex, int pageSize);

        public PageResult<T> FilterPaged(Expression<Func<T, bool>> filterExpression, int pageIndex, int pageSize);

        public T Create(T t);

        public bool CreateMany(IEnumerable<T> tList);

        public T Update(T t);

        public bool Remove(string id);

        public bool Remove(string[] ids);

        public bool Remove(Expression<Func<T,bool>> filterExpression);

    }
}
