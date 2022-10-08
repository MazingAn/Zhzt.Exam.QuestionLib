
using Nacos.AspNetCore.V2;
using SqlSugar.Extension.DomainHelper;
using SqlSugar.Extensions.CodeFirst;
using Zhzt.Exam.Auth.DomainInterface;
using Zhzt.Exam.Auth.DomainService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.DateFormatString = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
    options.SerializerSettings.ContractResolver = new SnowflakeNewtonJsonResolver();
});

// Add Nacos Support to load configuration from nacos server
builder.Host.ConfigureAppConfiguration((context, builder) =>
{
    var c = builder.Build();
    builder.AddNacosV2Configuration(c.GetSection("NacosConfig"));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// SqlSugar����
builder.Services.AddSqlSugarWithRedisCacheSetup(builder.Configuration);
// SqlSugar��ѩ��ID����
builder.Services.AddSqlSugarSonwFlakeSetup(builder.Configuration);
// SqlSugarCodeFirst����
builder.Services.AddCodeFirstSetup(builder.Configuration, typeof(BaseModel));

//�����ע��
builder.Services.AddTransient<IUserService, UserService>();

// Regist this service to nacos server
builder.Services.AddNacosAspNet(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
