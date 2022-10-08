using Microsoft.AspNetCore.Http.Features;
using Nacos.AspNetCore.V2;
using SqlSugar.Extension.DomainHelper;
using SqlSugar.Extensions.CodeFirst;
using Zhzt.Exam.Document.DomainInterface;
using Zhzt.Exam.Document.DomainService;
using Zhzt.Exam.StaticFileSystem;

var builder = WebApplication.CreateBuilder(args);

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

// ���Ӿ�̬�ļ�Ŀ¼����
builder.Services.AddFileSystemConfig(builder.Configuration);

// SqlSugar����
builder.Services.AddSqlSugarWithRedisCacheSetup(builder.Configuration);
// SqlSugar��ѩ��ID����
builder.Services.AddSqlSugarSonwFlakeSetup(builder.Configuration);
// SqlSugarCodeFirst����
builder.Services.AddCodeFirstSetup(builder.Configuration, typeof(BaseModel));

//����ע��
builder.Services.AddTransient<IDocumentCategoryService, DocumentCategoryService>();
builder.Services.AddTransient<IDocumentService, DocumentService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Nacos����ע��
builder.Services.AddNacosAspNet(builder.Configuration);
builder.Services.Configure<FormOptions>(x =>
{
    x.ValueLengthLimit = int.MaxValue;
    x.MultipartBodyLengthLimit = int.MaxValue;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.Services.GetService<IDocumentService>()?.CreateSeedData();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
