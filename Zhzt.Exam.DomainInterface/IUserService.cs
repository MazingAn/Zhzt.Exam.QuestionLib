using SqlSugar.Extension.DomainHelper;
using System.Linq.Expressions;

namespace Zhzt.Exam.DomainInterface
{
    public interface IUserService : IBaseService
    {
        public User Regist(RegisterModel regModel);
        public bool ChangePassword(string username, string password, string newPassword);
        public bool Validate(string username, string password);
        public User AttachDepartmentInfo(User user);
        public User AttachRoleInfo(User user);
        public IEnumerable<User> FilterAndAttach(Expression<Func<User, bool>> filterExpression);
        public PageResult<User> FilterPageAndAttach(int pageIndex, int pageSize, Expression<Func<User, bool>> filterExpression);
        public PageResult<User> GetPageAndAttach(int pageIndex, int pageSize, Expression<Func<User, object>> order);
        public User GetByIdAndAttach(long id);
    }
}