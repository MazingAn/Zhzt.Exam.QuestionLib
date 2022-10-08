using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Netcore.Extensions.WebModels;
using Zhzt.Exam.Document.DomainInterface;
using Zhzt.Exam.Document.DomainModel;
using Zhzt.Exam.DocumentLib.Api.Models;
using Zhzt.Exam.StaticFileSystem;

namespace Zhzt.Exam.FileDocumentLib.Api.Controllers
{
    [Route("api/documentlib/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentservice;
        private readonly IDocumentCategoryService _documentcategoryservice;
        private readonly FileSystemSettings _staticFileSettings;

        public DocumentController(IDocumentService docservice,
            IDocumentCategoryService videocategoryservice,
            IOptions<FileSystemSettings> staticFIleSettings)
        {
            _documentservice = docservice;
            _documentcategoryservice = videocategoryservice;
            _staticFileSettings = staticFIleSettings.Value;
        }

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="doc">对象实体</param>
        /// <returns>创建后的对象</returns>
        [HttpPost("create")]
        public HttpJsonResponse Create(FileDocument doc)
        {
            try
            {
                if (!string.IsNullOrEmpty(doc.DocUrl))
                {
                    string thumbName;
                    string pdfName;
                    string pdfOutPath;
                    string input = Path.Combine(_staticFileSettings.StaticServerRoot, doc.DocUrl);
                    if (Path.GetExtension(input).ToLower().Equals(".pdf"))
                    {
                        pdfName = doc.DocUrl;
                        pdfOutPath = Path.Combine(_staticFileSettings.StaticServerRoot, pdfName);
                    }
                    else
                    {
                        pdfName = Guid.NewGuid().ToString() + ".pdf";
                        pdfOutPath = Path.Combine(_staticFileSettings.StaticServerRoot, pdfName);
                        _documentservice.GenPdf(input, pdfOutPath);
                    }
                    thumbName = Guid.NewGuid().ToString() + ".jpg";
                    var thumbPath = Path.Combine(_staticFileSettings.StaticServerRoot, thumbName);
                    // Donet 的社区还是存在很多问题，小众玩意儿导致linux上图形库逐渐也不支持
                    // 暂时没办法在Linux上比较高效和完美的生成缩略图 
                    //_documentservice.GenThumb(pdfOutPath, thumbPath);
                    doc.Thumb = thumbName;
                    doc.PdfUrl = pdfName;
                }
                var data = _documentservice.Save(doc);
                return data is null ?
                    HttpJsonResponse.FailedResult("创建失败") :
                    HttpJsonResponse.SuccessResult(data);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
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
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(upFile.FileName);
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
        /// <param name="doc"></param>
        /// <returns>变更后的数据</returns>
        [HttpPut("update")]
        public HttpJsonResponse Update(FileDocument doc)
        {
            try
            {
                if (!string.IsNullOrEmpty(doc.DocUrl))
                {
                    string thumbName;
                    string pdfName;
                    string pdfOutPath;
                    string input = Path.Combine(_staticFileSettings.StaticServerRoot, doc.DocUrl);
                    if (Path.GetExtension(input).ToLower().Equals(".pdf"))
                    {
                        pdfName = doc.DocUrl;
                        pdfOutPath = Path.Combine(_staticFileSettings.StaticServerRoot, pdfName);
                    }
                    else
                    {
                        pdfName = Guid.NewGuid().ToString() + ".pdf";
                        pdfOutPath = Path.Combine(_staticFileSettings.StaticServerRoot, pdfName);
                        _documentservice.GenPdf(input, pdfOutPath);
                    }
                    thumbName = Guid.NewGuid().ToString() + ".jpg";
                    var thumbPath = Path.Combine(_staticFileSettings.StaticServerRoot, thumbName);
                    // Donet 的社区还是存在很多问题，小众玩意儿导致linux上图形库逐渐也不支持
                    // 暂时没办法在Linux上比较高效和完美的生成缩略图 
                    //_documentservice.GenThumb(pdfOutPath, thumbPath);
                    doc.Thumb = thumbName;
                    doc.PdfUrl = pdfName;
                }
                var data = _documentservice.Update(doc);
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
                bool success = _documentservice.Delete<FileDocument>(id);
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
                bool success = _documentservice.Delete<FileDocument>(ids.Ids);
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
                var data = _documentservice.GetAll<FileDocument>();
                _documentservice.AttachCategory(data);
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
                var data = _documentservice.GetPage<FileDocument>(pageIndex, pageSize, o => o.CreateTime);
                _documentservice.AttachCategory(data.PageData);
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
        public HttpJsonResponse Filter(FileDocumentFilter filterObj)
        {
            try
            {
                var data = _documentservice.Filter<FileDocument>(filterObj.GetFilterExpression(_documentcategoryservice));
                _documentservice.AttachCategory(data);
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
        public HttpJsonResponse GetPagedByFilter(FileDocumentFilter filter)
        {
            try
            {
                var data = _documentservice.FilterPage<FileDocument>(filter.PageIndex, filter.PageSize, filter.GetFilterExpression(_documentcategoryservice));
                _documentservice.AttachCategory(data.PageData);
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
                int? count = _documentservice.Count<FileDocument>();
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
        public HttpJsonResponse CountAll(FileDocumentFilter filter)
        {
            try
            {
                int count = _documentservice.Count<FileDocument>(filter.GetFilterExpression(_documentcategoryservice));
                return HttpJsonResponse.SuccessResult(count);
            }
            catch
            {
                return HttpJsonResponse.FailedResult("查询失败");
            }
        }
    }
}
