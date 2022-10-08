using SqlSugar;
using System.Linq.Expressions;
using Zhzt.Exam.MicroClass.DomainModel;

namespace Zhzt.Exam.MicroClassLib.Api.Models
{
    public class VideoCategoryFilter
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public long ParentId { get; set; } = -1;

        /// <summary>
        /// 对象转表达式
        /// </summary>
        /// <returns></returns>
        public Expression<Func<VideoCategory, bool>> GetFilterExpression()
        {
            return Expressionable.Create<VideoCategory>()
                .AndIF(ParentId != -1, l => l.ParentId == ParentId)
                .ToExpression();
        }
    }
}
