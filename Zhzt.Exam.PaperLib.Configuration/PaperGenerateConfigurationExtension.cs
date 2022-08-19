using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhzt.Exam.PaperLib.Configuration
{
    public static class PaperGenerateConfigurationExtension
    {
        public static IServiceCollection AddPaperGenerateConfig(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<PaperGenerateSettings>(config.GetSection("PaperGenerateSettings"));
            return services;
        }
    }
}
