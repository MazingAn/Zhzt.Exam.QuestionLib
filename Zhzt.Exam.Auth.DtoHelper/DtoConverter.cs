using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zhzt.Exam.Auth.DomainDtoModel;
using Zhzt.Exam.Auth.DomainModel;

namespace Zhzt.Exam.Auth.DomainDtoHelper
{
    public static class DtoConverter
    {
        public static User RegModelToUser(RegisterDto regModel)
        {
            return new User
            {
                Username = regModel.Username,
                Password = regModel.Password,
                DepartmentId = regModel.DepartmentId,
                Realname = regModel.RealName
            };
        }
    }
}
