using SqlSugar;
using System.Linq.Expressions;
using Zhzt.Exam.MicroClass.DomainInterface;
using Zhzt.Exam.MicroClass.DomainModel;

namespace Zhzt.Exam.MicroClassLib.Api.Models
{
    public class MicroClassVideoFilter
    {
        public int PageIndex { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public long CategoryId { get; set; } = 0;

        /// <summary>
        /// 对象转表达式
        /// </summary>
        /// <returns></returns>
        public Expression<Func<MicroClassVideo, bool>> GetFilterExpression(IVideoCategoryService? service)
        {
            List<long> matchIds = new List<long>();
            matchIds.Add(CategoryId);
            var allChilds = service?.GetAllChildren<VideoCategory>(CategoryId);
            if (allChilds?.Count() > 0)
            {
                matchIds = matchIds.Concat(allChilds.Select(x => x.Id).ToList()).ToList();
            }
            return Expressionable.Create<MicroClassVideo>()
                .AndIF(CategoryId != 0, l => matchIds.Contains(l.VideoCategoryId))
                .ToExpression();
        }
    }
}
