using Microsoft.AspNetCore.Mvc;
using Netcore.Extensions.WebModels;
using Zhzt.Exam.Auth.Api.Model;
using Zhzt.Exam.Auth.DomainDtoModel;
using Zhzt.Exam.Auth.DomainInterface;
using Zhzt.Exam.Auth.DomainModel;

namespace Zhzt.Exam.Auth.Api.Controllers
{
    [Route("api/authcenter/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// 注册用户
        /// </summary>
        [HttpPost("create")]
        public HttpJsonResponse Registry(RegisterDto regModel)
        {
            try
            {
                var user = _userService?.Regist(regModel);
                return HttpJsonResponse.SuccessResult(user);
            }
            catch
            {
                return HttpJsonResponse.FailedResult("删除数据失败");
            }
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        [HttpPut("update")]
        public HttpJsonResponse Update(User user)
        {
            try
            {
                var oldUser = _userService?.GetById<User>(user.Id);
                if (oldUser != null)
                {
                    user.Password = oldUser.Password;
                    var ret = _userService?.Update(user);
                    return HttpJsonResponse.SuccessResult(user);
                }
                return HttpJsonResponse.FailedResult("不存在的用户");
            }
            catch
            {
                return HttpJsonResponse.FailedResult("删除数据失败");
            }
        }

        /// <summary>
        /// 根据Id删除数据
        /// </summary>
        /// <param name="id">数据id</param>
        /// <returns>是否删除成功的响应</returns>
        [HttpDelete("delete")]
        public HttpJsonResponse Delete(long id)
        {
            try
            {
                bool success = _userService?.Delete<User>(id) ?? false;
                return success ?
                    HttpJsonResponse.SuccessResult(true, "删除数据成功") :
                    HttpJsonResponse.FailedResult("删除数据失败");
            }
            catch
            {
                return HttpJsonResponse.FailedResult("删除数据失败");
            }

        }

        /// <summary>
        /// 根据Id数组删除数据
        /// </summary>
        /// <param name="id">数据id</param>
        /// <returns>是否删除成功的响应</returns>
        [HttpDelete("deletemany")]
        public HttpJsonResponse Delete(DeleteIds ids)
        {
            try
            {
                bool success = _userService?.Delete<User>(ids.Ids) ?? false;
                return success ?
                    HttpJsonResponse.SuccessResult(true, "删除数据成功") :
                    HttpJsonResponse.FailedResult("删除数据失败");
            }
            catch
            {
                return HttpJsonResponse.FailedResult("删除数据失败");
            }

        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>数据集合</returns>
        [HttpGet("all")]
        public HttpJsonResponse GetAll()
        {
            try
            {
                var data = _userService?.GetAll<User>();
                return HttpJsonResponse.SuccessResult(data);
            }
            catch
            {
                return HttpJsonResponse.FailedResult("查询数据失败");
            }
        }

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页尺寸</param>
        /// <returns>分页结果</returns>
        [HttpGet("page")]
        public HttpJsonResponse GetPaged(int pageIndex, int pageSize)
        {
            try
            {
                var data = _userService?.GetPageAndAttach(pageIndex, pageSize, o => o.CreateTime);
                return HttpJsonResponse.SuccessResult(data);
            }
            catch
            {
                return HttpJsonResponse.FailedResult("查询数据失败");
            }
        }

        [HttpGet("getone/{id}")]
        public HttpJsonResponse GetOneById(long id)
        {
            try
            {
                var data = _userService?.GetByIdAndAttach(id);
                return HttpJsonResponse.SuccessResult(data);
            }
            catch
            {
                return HttpJsonResponse.FailedResult("查询数据失败");
            }
        }

        /// <summary>
        /// 筛选数据
        /// </summary>
        /// <param name="filterObj">筛选条件对象</param>
        /// <returns>筛选结果</returns>
        [HttpPost("filter")]
        public HttpJsonResponse Filter(UserFilter filterObj)
        {
            try
            {
                var data = _userService?.FilterAndAttach(filterObj.GetFilterExpression());
                return HttpJsonResponse.SuccessResult(data);
            }
            catch
            {
                return HttpJsonResponse.FailedResult("执行筛选操作失败");
            }
        }

        /// <summary>
        /// 按条件分页筛选数据
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>筛选结果</returns>
        [HttpPost("filterpage")]
        public HttpJsonResponse GetPagedByFilter(UserFilter filter)
        {
            try
            {
                var data = _userService?.FilterPageAndAttach(filter.PageIndex, filter.PageSize, filter.GetFilterExpression());
                return HttpJsonResponse.SuccessResult(data);
            }
            catch
            {
                return HttpJsonResponse.FailedResult("执行数据筛选失败");
            }

        }

        /// <summary>
        /// 查询总数
        /// </summary>
        /// <returns>数量</returns>
        [HttpGet("count")]
        public HttpJsonResponse CountAll()
        {
            try
            {
                int? count = _userService?.Count<User>();
                return HttpJsonResponse.SuccessResult(count);
            }
            catch
            {
                return HttpJsonResponse.FailedResult("查询失败");
            }
        }

        /// <summary>
        /// 筛选查询总数
        /// </summary>
        /// <returns>数量</returns>
        [HttpPost("count/filter")]
        public HttpJsonResponse CountAll(UserFilter filter)
        {
            try
            {
                int count = _userService?.Count<User>(filter.GetFilterExpression()) ?? 0;
                return HttpJsonResponse.SuccessResult(count);
            }
            catch
            {
                return HttpJsonResponse.FailedResult("查询失败");
            }
        }

    }
}
