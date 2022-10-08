using Netcore.Extensions.WebModels;
using SqlSugar;
using SqlSugar.Extension.DomainHelper;
using System.Data;
using System.Linq.Expressions;
using Zhzt.Exam.Auth.DomainDtoHelper;
using Zhzt.Exam.Auth.DomainDtoModel;
using Zhzt.Exam.Auth.DomainInterface;
using Zhzt.Exam.Auth.DomainModel;

namespace Zhzt.Exam.Auth.DomainService
{
    public class UserService : BaseService, IUserService
    {
        public UserService(ISqlSugarClient client) : base(client)
        {
        }

        /// <summary>
        /// 附加部门信息
        /// </summary>
        /// <param name="user">要附加的User</param>
        /// <returns>user</returns>
        public User AttachDepartmentInfo(User user)
        {
            if (user.DepartmentId != -1)
            {
                //RODO1: Load Department Info By RPC
            }
            return user;
        }

        /// <summary>
        /// 附加用户的角色信息
        /// </summary>
        /// <param name="user">要附加的User</param>
        /// <returns>user</returns>
        public User AttachRoleInfo(User user)
        {
            if (user.RoleId != -1)
            {
                //TODO2: Load Department Info By RPC
            }
            return user;
        }

        /// <summary>
        /// 筛选并且附加外键信息
        /// </summary>
        /// <param name="filterExpression"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IEnumerable<User> FilterAndAttach(Expression<Func<User, bool>> filterExpression)
        {
            var results = Filter<User>(filterExpression);
            if (results != null)
            {
                foreach (var user in results)
                {
                    AttachDepartmentInfo(user);
                    AttachRoleInfo(user);
                }
                return results;
            }
            throw new Exception("没有找到符合条件的用户.");
        }

        /// <summary>
        /// 分页筛选并附加外键信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="filterExpression"></param>
        /// <returns></returns>
        public PageResult<User> FilterPageAndAttach(int pageIndex, int pageSize, Expression<Func<User, bool>> filterExpression)
        {
            var results = FilterPage(pageIndex, pageSize, filterExpression);
            if (results?.PageData != null)
            {
                foreach (var user in results.PageData)
                {
                    AttachDepartmentInfo(user);
                    AttachRoleInfo(user);
                }
                return results;
            }
            throw new Exception("没有找到符合条件的用户.");
        }

        /// <summary>
        /// 分页查找并附加外键数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public PageResult<User> GetPageAndAttach(int pageIndex, int pageSize, Expression<Func<User, object>> order)
        {
            var results = GetPage(pageIndex, pageSize, order);
            if (results?.PageData != null)
            {
                foreach (var user in results.PageData)
                {
                    AttachDepartmentInfo(user);
                    AttachRoleInfo(user);
                }
                return results;
            }
            throw new Exception("无数据.");
        }

        /// <summary>
        /// 根据id查询并附加外键信息
        /// </summary>
        /// <param name="filterExpression"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public User GetByIdAndAttach(long id)
        {
            var user = GetById<User>(id);
            if (user != null)
            {
                AttachDepartmentInfo(user);
                AttachRoleInfo(user);
                return user;
            }
            throw new Exception($"没有找到此ID（{id}）对应的用户.");
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public bool ChangePassword(string username, string password, string newPassword)
        {
            if (Validate(username, password))
            {
                User user = _client?.Queryable<User>().Single(x => x.Username == username)!;
                user.Password = PasswordEncoder.EncryptPassword(newPassword);
                Update(user);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="regModel"></param>
        /// <returns></returns>
        public User Regist(RegisterDto regModel)
        {
            User user = DtoConverter.RegModelToUser(regModel);
            user.Password = PasswordEncoder.EncryptPassword(user.Password);
            Save(user);
            return user;
        }

        /// <summary>
        /// 用户验证
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool Validate(string username, string password)
        {
            return _client?.Queryable<User>().Any(x => x.Username == username && x.Password == PasswordEncoder.EncryptPassword(password)) ?? false;
        }
    }
}