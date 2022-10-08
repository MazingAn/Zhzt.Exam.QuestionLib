using SqlSugar;
using SqlSugar.Extension.DomainHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zhzt.Exam.MicroClass.DomainInterface;

namespace Zhzt.Exam.MicroClass.DomainService
{
    public class VideoCategoryService : BaseCachedService, IVideoCategoryService
    {
        public VideoCategoryService(ISqlSugarClient client) : base(client)
        {
        }
    }
}
