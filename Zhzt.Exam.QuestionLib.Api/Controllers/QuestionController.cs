using Microsoft.AspNetCore.Mvc;
using SqlsugarCodeFirst.WebModel;
using Zhzt.Exam.QuestionLib.DomainInterface;
using Zhzt.Exam.QuestionLib.DomainModel;

namespace Zhzt.Exam.QuestionLib.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly ILogger<QuestionController> _logger;
        private readonly IQuestionService _questionTypeService;

        public QuestionController(ILogger<QuestionController> logger, IQuestionService questionTypeService)
        {
            _logger = logger;
            _questionTypeService = questionTypeService;
        }

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="questionType">对象实体</param>
        /// <returns>创建后的对象</returns>
        [HttpPost("create")]
        public HttpJsonResponse Create(Question questionType)
        {
            try
            {
                var data = _questionTypeService?.Save(questionType);
                return data is null ?
                    HttpJsonResponse.FailedResult("创建失败") :
                    HttpJsonResponse.SuccessResult(data);
            }
            catch
            {
                return HttpJsonResponse.FailedResult("创建失败");
            }
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="questionType"></param>
        /// <returns>变更后的数据</returns>
        [HttpPut("update")]
        public HttpJsonResponse Update(Question questionType)
        {
            try
            {
                var data = _questionTypeService?.Update(questionType);
                return HttpJsonResponse.SuccessResult(data);
            }
            catch
            {
                return HttpJsonResponse.FailedResult("更新数据失败");
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
                bool success = _questionTypeService?.Delete<Question>(id) ?? false;
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
                bool success = _questionTypeService?.Delete<Question>(ids.Ids) ?? false;
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
                var data = _questionTypeService?.GetAll<Question>();
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
                var data = _questionTypeService?.GetPage<Question>(pageIndex, pageSize, o => o.CreateTime);
                return HttpJsonResponse.SuccessResult(data);
            }
            catch
            {
                return HttpJsonResponse.FailedResult("查询数据失败");
            }
        }

        /*
        /// <summary>
        /// 筛选数据
        /// </summary>
        /// <param name="filterObj">筛选条件对象</param>
        /// <returns>筛选结果</returns>
        [HttpPost("filter")]
        public HttpJsonResponse Filter(QuestionFilter filterObj)
        {
            try
            {
                var data = _questionTypeService?.Filter<Question>(filterObj.GetFilterExpression());
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
        public HttpJsonResponse GetPagedByFilter(QuestionFilter filter)
        {
            // FIXME 请根据需求自行创建QuestionFilter对象
            try
            {
                var data = _questionTypeService?.FilterPage<Question>(filter.PageIndex, filter.PageSize, filter.GetFilterExpression());
                return HttpJsonResponse.SuccessResult(data);
            }
            catch
            {
                return HttpJsonResponse.FailedResult("执行数据筛选失败");
            }

        }
        */

        /// <summary>
        /// 查询总数
        /// </summary>
        /// <returns>数量</returns>
        [HttpGet("count")]
        public HttpJsonResponse CountAll()
        {
            try
            {
                int? count = _questionTypeService?.Count<Question>();
                return HttpJsonResponse.SuccessResult(count);
            }
            catch
            {
                return HttpJsonResponse.FailedResult("查询失败");
            }
        }

        /*
        /// <summary>
        /// 筛选查询总数
        /// </summary>
        /// <returns>数量</returns>
        [HttpPost("count/filter")]
        public HttpJsonResponse CounAll(QuestionFilter filter)
        {
            try
            {
                int? count = _questionTypeService?.Count<Question>(filter.GetFilterExpression());
                return HttpJsonResponse.SuccessResult(count);
            }
            catch
            {
                return HttpJsonResponse.FailedResult("查询失败");
            }
        }
        */
    }
}