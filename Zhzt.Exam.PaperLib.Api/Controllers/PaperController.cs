using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Netcore.Extensions.WebModels;
using Zhzt.Exam.PaperLib.Api.Models;
using Zhzt.Exam.PaperLib.Configuration;
using Zhzt.Exam.PaperLib.DomainInterface;
using Zhzt.Exam.PaperLib.DomainModel;

namespace Zhzt.Exam.PaperLib.Api.Controllers
{
    [ApiController]
    [Route("api/paperlib/[controller]")]
    public class PaperController : ControllerBase
    {
        private readonly IPaperService _paperService;
        private readonly PaperGenerateSettings _paperGenerateSettings;
        private readonly IPaperGenerate _paperGenerator;

        public PaperController(IPaperService paperService, IPaperGenerate paperGenerator, IOptions<PaperGenerateSettings> generateConfig)
        {
            _paperService = paperService;
            _paperGenerator = paperGenerator;
            _paperGenerateSettings = generateConfig.Value;
        }

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="paper">对象实体</param>
        /// <returns>创建后的对象</returns>
        [HttpPost("create")]
        public HttpJsonResponse Create(DocPaper paper)
        {
            try
            {
                // 构造
                var genPaper = _paperService.GenerateQuestions(paper);
                // 保存
                genPaper = _paperService?.Create(genPaper);
                // 生成试卷文件
                string paperFilePath = _paperGenerator.GeneratePaper(genPaper!) ?? string.Empty;
                genPaper!.PaperFilePath = paperFilePath;
                var data = _paperService?.Update(genPaper);
                return data is null ?
                    HttpJsonResponse.FailedResult("创建失败") :
                    HttpJsonResponse.SuccessResult(data);
            }
            catch
            {
                return HttpJsonResponse.FailedResult("创建失败,请检查是否有足够的题目");
            }
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="paper"></param>
        /// <returns>变更后的数据</returns>
        [HttpPut("update")]
        public HttpJsonResponse Update(DocPaper paper)
        {
            try
            {
                var genPaper = _paperService.GenerateQuestions(paper);
                string paperFilePath = _paperGenerator.GeneratePaper(genPaper) ?? string.Empty;
                genPaper.PaperFilePath = paperFilePath;
                var data = _paperService.Update(genPaper);
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
        public HttpJsonResponse Delete(string id)
        {
            try
            {
                bool success = _paperService?.Remove(id) ?? false;
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
        public HttpJsonResponse Delete(DeleteStrIds ids)
        {
            try
            {
                bool success = _paperService?.Remove(ids.Ids.ToArray()) ?? false;
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
                var data = _paperService?.GetAll();
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
                var data = _paperService?.GetPaged(pageIndex, pageSize);
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
        public HttpJsonResponse Filter(DocPaperFilter filterObj)
        {
            try
            {
                var data = _paperService?.FilterAll(filterObj);
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
        public HttpJsonResponse GetPagedByFilter(DocPaperFilter filter)
        {
            // FIXME 请根据需求自行创建DocPaperFilter对象
            try
            {
                var data = _paperService?.FilterPaged(filter, filter.PageIndex, filter.PageSize);
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
                int? count = _paperService?.Count();
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
        public HttpJsonResponse CounFilter(DocPaperFilter filter)
        {
            try
            {
                int? count = _paperService?.Count(filter);
                return HttpJsonResponse.SuccessResult(count);
            }
            catch
            {
                return HttpJsonResponse.FailedResult("查询失败");
            }
        }

        [HttpGet("download/{id}")]
        public FileResult? DownloadFile([FromRoute] string id)
        {
            var paper = _paperService.GetOneById(id);
            string path = Path.Combine(_paperGenerateSettings.BaseDir, paper.PaperFilePath ?? "");
            if (System.IO.File.Exists(path))
            {
                return PhysicalFile(path, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", $"{paper.Name}.docx");
            }
            else
            {
                return null;
            }
        } 

    }
}
