using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhzt.Exam.StaticFileSystem
{
    public static class MyFileSystemExtension
    {
        public static IServiceCollection AddFileSystemConfig(this IServiceCollection services, IConfiguration conf)
        {
            services.Configure<FileSystemSettings>(conf.GetSection("FileSystemSettings"));
            return services;
        }
    }
}
