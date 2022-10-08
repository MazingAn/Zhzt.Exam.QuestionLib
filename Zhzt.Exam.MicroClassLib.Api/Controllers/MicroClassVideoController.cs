using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Netcore.Extensions.WebModels;
using Zhzt.Exam.MicroClass.DomainInterface;
using Zhzt.Exam.MicroClass.DomainModel;
using Zhzt.Exam.MicroClassLib.Api.Models;
using Zhzt.Exam.StaticFileSystem;

namespace Zhzt.Exam.MicroClassVideoLib.Api.Controllers
{
    [Route("api/microclasslib/[controller]")]
    [ApiController]
    public class MicroClassVideoController : ControllerBase
    {
        private readonly IMicroClassVideoService _microclassservice;
        private readonly IVideoCategoryService _videocategoryservice;
        private readonly FileSystemSettings _staticFileSettings;

        public MicroClassVideoController(IMicroClassVideoService microclassservice,
            IVideoCategoryService videocategoryservice,
            IOptions<FileSystemSettings> staticFIleSettings)
        {
            _microclassservice = microclassservice;
            _videocategoryservice = videocategoryservice;
            _staticFileSettings = staticFIleSettings.Value;
        }

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="microclass">对象实体</param>
        /// <returns>创建后的对象</returns>
        [HttpPost("create")]
        public HttpJsonResponse Create(MicroClassVideo microclass)
        {
            try
            {
                if (!string.IsNullOrEmpty(microclass.VideoUrl))
                {
                    var thumbName = Guid.NewGuid().ToString() + ".png";
                    var thumbPath = Path.Combine(_staticFileSettings.StaticServerRoot, thumbName);
                    var videoPath = Path.Combine(_staticFileSettings.StaticServerRoot, microclass.VideoUrl);
                    _microclassservice.GenThumbAsync(videoPath, thumbPath);
                    microclass.Thumb = thumbName;
                }
                var data = _microclassservice.Save(microclass);
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
        /// 上传视频
        /// </summary>
        /// <returns></returns>
        [HttpPost("upload")]
        [RequestSizeLimit(int.MaxValue)]
        public HttpJsonResponse Upload()
        {
            try
            {
                IFormFile upFile = Request.Form.Files.Last();
                if (upFile == null || upFile.Length == 0)
                    return HttpJsonResponse.FailedResult("上传失败");
                var fileName = Guid.NewGuid().ToString() + ".mp4";
                var filePath = Path.Combine(_staticFileSettings.StaticServerRoot, fileName);
                var videoUrl = fileName;
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    upFile.CopyTo(stream);
                }
                return HttpJsonResponse.SuccessResult(videoUrl);
            }
            catch(Exception ex)
            {
                return HttpJsonResponse.FailedResult("创建失败");
            }
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="microclass"></param>
        /// <returns>变更后的数据</returns>
        [HttpPut("update")]
        public HttpJsonResponse Update(MicroClassVideo microclass)
        {
            try
            {
                if (!string.IsNullOrEmpty(microclass.VideoUrl))
                {
                    var thumbName = Guid.NewGuid().ToString() + ".png";
                    var thumbPath = Path.Combine(_staticFileSettings.StaticServerRoot, thumbName);
                    var videoPath = Path.Combine(_staticFileSettings.StaticServerRoot, microclass.VideoUrl);
                    _microclassservice.GenThumbAsync(videoPath, thumbPath);
                    microclass.Thumb = thumbName;
                }
                var data = _microclassservice.Update(microclass);
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
                bool success = _microclassservice.Delete<MicroClassVideo>(id);
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
                bool success = _microclassservice.Delete<MicroClassVideo>(ids.Ids);
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
                var data = _microclassservice.GetAll<MicroClassVideo>();
                _microclassservice.AttachVideoCategory(data);
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
                var data = _microclassservice.GetPage<MicroClassVideo>(pageIndex, pageSize, o => o.CreateTime);
                _microclassservice.AttachVideoCategory(data.PageData);
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
        public HttpJsonResponse Filter(MicroClassVideoFilter filterObj)
        {
            try
            {
                var data = _microclassservice.Filter<MicroClassVideo>(filterObj.GetFilterExpression(_videocategoryservice));
                _microclassservice.AttachVideoCategory(data);
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
        public HttpJsonResponse GetPagedByFilter(MicroClassVideoFilter filter)
        {
            try
            {
                var data = _microclassservice.FilterPage<MicroClassVideo>(filter.PageIndex, filter.PageSize, filter.GetFilterExpression(_videocategoryservice));
                _microclassservice.AttachVideoCategory(data.PageData);
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
                int? count = _microclassservice.Count<MicroClassVideo>();
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
        public HttpJsonResponse CountAll(MicroClassVideoFilter filter)
        {
            try
            {
                int count = _microclassservice.Count<MicroClassVideo>(filter.GetFilterExpression(_videocategoryservice));
                return HttpJsonResponse.SuccessResult(count);
            }
            catch
            {
                return HttpJsonResponse.FailedResult("查询失败");
            }
        }
    }
}
