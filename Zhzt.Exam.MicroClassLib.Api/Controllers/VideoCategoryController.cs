using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Netcore.Extensions.WebModels;
using Zhzt.Exam.MicroClass.DomainInterface;
using Zhzt.Exam.MicroClass.DomainModel;
using Zhzt.Exam.MicroClassLib.Api.Models;

namespace Zhzt.Exam.MicroClassLib.Api.Controllers
{
    [Route("api/microclasslib/[controller]")]
    [ApiController]
    public class VideoCategoryController : ControllerBase
    {
        private readonly IVideoCategoryService? _videoCategoryService;

        public VideoCategoryController(IVideoCategoryService? videoCategoryService)
        {
            _videoCategoryService = videoCategoryService;
        }

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="videocategory">对象实体</param>
        /// <returns>创建后的对象</returns>
        [HttpPost("create")]
        public HttpJsonResponse Create(VideoCategory videocategory)
        {
            try
            {
                var data = _videoCategoryService?.Save(videocategory);
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
        /// <param name="videocategory"></param>
        /// <returns>变更后的数据</returns>
        [HttpPut("update")]
        public HttpJsonResponse Update(VideoCategory videocategory)
        {
            try
            {
                var data = _videoCategoryService?.Update(videocategory);
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
                bool success = _videoCategoryService?.Delete<VideoCategory>(id) ?? false;
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
                bool success = _videoCategoryService?.Delete<VideoCategory>(ids.Ids) ?? false;
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
        [HttpGet("all/tree")]
        public HttpJsonResponse GetAllTree()
        {
            try
            {
                var data = _videoCategoryService?.GetAllTree<VideoCategory>();
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
        [HttpGet("page/tree")]
        public HttpJsonResponse GetTreePaged(int pageIndex, int pageSize)
        {
            try
            {
                var data = _videoCategoryService?.GetPageTree<VideoCategory>(pageIndex, pageSize);
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
        public HttpJsonResponse Filter(VideoCategoryFilter filterObj)
        {
            try
            {
                var data = _videoCategoryService?.Filter<VideoCategory>(filterObj.GetFilterExpression());
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
        public HttpJsonResponse GetPagedByFilter(VideoCategoryFilter filter)
        {
            try
            {
                var data = _videoCategoryService?.FilterPage<VideoCategory>(filter.PageIndex, filter.PageSize, filter.GetFilterExpression());
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
                int? count = _videoCategoryService?.Count<VideoCategory>();
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
        public HttpJsonResponse CountAll(VideoCategoryFilter filter)
        {
            try
            {
                int? count = _videoCategoryService?.Count<VideoCategory>(filter.GetFilterExpression());
                return HttpJsonResponse.SuccessResult(count);
            }
            catch
            {
                return HttpJsonResponse.FailedResult("查询失败");
            }
        }
    }
}
