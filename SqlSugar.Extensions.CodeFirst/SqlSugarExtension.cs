using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System.Reflection;

namespace SqlSugar.Extensions.CodeFirst
{
    public static class SqlSugarExtension
    {
        private static SqlSugarScope? _sqlSugarClient;

        /// <summary>
        /// ServiceCollection 拓展方法  配置sqlsugar 注入SqlSugarClient（相当于数据库游标）
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddSqlSugarSetup(this IServiceCollection services, IConfiguration configuration)
        {
            _sqlSugarClient = new SqlSugarScope(new ConnectionConfig()
            {
                DbType = DbType.MySql,
                ConnectionString = configuration.GetConnectionString("MySqlConn"),
                IsAutoCloseConnection = true,
            }, db =>
            {
                db.Aop.OnLogExecuting = (sql, args) =>
                {
                    Console.WriteLine(sql);
                };
            });
            services.AddSingleton<ISqlSugarClient>(_sqlSugarClient);
        }

        public static void AddSqlSugarWithRedisCacheSetup(this IServiceCollection services, IConfiguration configuration)
        {
            SqlSugarRedisCache cacheService = new SqlSugarRedisCache(configuration.GetConnectionString("RedisConn"));
            _sqlSugarClient = new SqlSugarScope(new ConnectionConfig()
            {
                DbType = DbType.MySql,
                ConnectionString = configuration.GetConnectionString("MySqlConn"),
                IsAutoCloseConnection = true,
                ConfigureExternalServices = new ConfigureExternalServices()
                {
                    DataInfoCacheService = cacheService
                }
            }, db =>
            {
                db.Aop.OnLogExecuting = (sql, args) =>
                {
                    Console.WriteLine(sql);
                };
            });
            services.AddSingleton<ISqlSugarClient>(_sqlSugarClient);
        }

        /// <summary>
        /// ServiceCollection CodeFirst 创建数据表
        /// </summary>
        /// <param name="backup"></param>
        /// <param name="stringDefaultLenght"></param>
        /// <param name="types"></param>
        public static void AddCodeFirstSetup(this IServiceCollection services, IConfiguration configuration, Type baseModelType)
        {
            // 读取配置文件 确定是否要进行CodeFirst同步
            if (bool.TryParse(configuration?.GetSection(CodeFirstSettingOptions.CodeFirstSettings)?["Migrate"],
                out bool migrate) && migrate)
            {
                // 读取配置文件里的备份开关
                bool.TryParse(configuration?.GetSection(CodeFirstSettingOptions.CodeFirstSettings)?["Backup"], out bool backup);
                // 读取配置文件内的模型名字空间配置
                string modelPath = configuration?.GetSection(CodeFirstSettingOptions.CodeFirstSettings)?["ModelPath"] ?? String.Empty;
                if (!string.IsNullOrWhiteSpace(modelPath))
                {
                    // 创建数据库
                    _sqlSugarClient?.DbMaintenance.CreateDatabase();

                    // 反射程序集
                    var asm = Assembly.Load(modelPath);
                    // 获取程序集下面的模型 【所有继承了BaseType的类 那肯定是模型类】
                    var types = asm.GetTypes().Where(a => a.IsSubclassOf(baseModelType)).ToArray();

                    if (backup)
                    {
                        _sqlSugarClient?.CodeFirst.BackupTable().InitTables(types);
                    }
                    else
                    {
                        _sqlSugarClient?.CodeFirst.InitTables(types);
                    }
                }
                else
                {
                    var modelPaths = configuration?.GetSection(CodeFirstSettingOptions.CodeFirstSettings).GetSection("ModelPath").GetChildren();
                    if (modelPaths is not null)
                    {
                        // 创建数据库
                        _sqlSugarClient?.DbMaintenance.CreateDatabase();
                        foreach (var item in modelPaths)
                        {
                            var modelPathMore = item.Value;
                            // 反射程序集
                            var asm = Assembly.Load(modelPathMore);
                            // 获取程序集下面的模型 【所有继承了BaseType的类 那肯定是模型类】
                            var types = asm.GetTypes().Where(a => a.IsSubclassOf(baseModelType)).ToArray();

                            if (backup)
                            {
                                _sqlSugarClient?.CodeFirst.BackupTable().InitTables(types);
                            }
                            else
                            {
                                _sqlSugarClient?.CodeFirst.InitTables(types);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 拓展方法  SqlSugar再带的雪花算法设置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddSqlSugarSonwFlakeSetup(this IServiceCollection services, IConfiguration configuration)
        {
            string workerIdStr = configuration?.GetSection(SqlSugarSnowFlakeSettingOption.SqlSugarSnowFlakeSettings)?["WorkerId"] ?? new Random().Next(31).ToString();
            int.TryParse(workerIdStr, out int workerId);
            SnowFlakeSingle.WorkId = workerId;
        }
    }
}