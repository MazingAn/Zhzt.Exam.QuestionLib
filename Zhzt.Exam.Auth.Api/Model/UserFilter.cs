using SqlSugar;
using System.Linq.Expressions;
using Zhzt.Exam.Auth.DomainModel;

namespace Zhzt.Exam.Auth.Api.Model
{
    /// <summary>
    /// 自定义筛选器 根据前端需求确定
    /// </summary>
    public class UserFilter
    {
        public int PageIndex { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string Username { get; set; } = string.Empty;

        public DateTime? StartRegTime { get; set; } = null;

        public DateTime? EndRegTime { get; set; } = null;


        /// <summary>
        /// 对象转表达式
        /// </summary>
        /// <returns></returns>
        public Expression<Func<User, bool>> GetFilterExpression()
        {
            return Expressionable.Create<User>()
                .AndIF(StartRegTime != null && EndRegTime != null,
                    l => l.CreateTime <= EndRegTime && l.CreateTime >= StartRegTime)
                .AndIF(!string.IsNullOrEmpty(Username),
                    l => l.Username.Contains(Username))
                .ToExpression();
        }
    }
}
