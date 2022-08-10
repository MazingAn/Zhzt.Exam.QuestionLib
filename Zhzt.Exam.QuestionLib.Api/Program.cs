using SqlSugar.Extensions.CodeFirst;
using SqlSugar.Extensions.DomainHelper;
using Zhzt.Exam.QuestionLib.DomainInterface;
using Zhzt.Exam.QuestionLib.DomainService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
  options.SerializerSettings.DateFormatString = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
  options.SerializerSettings.ContractResolver = new SnowflakeNewtonJsonResolver();
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// SqlSugar配置
builder.Services.AddSqlSugarWithRedisCacheSetup(builder.Configuration);
// SqlSugar的雪花ID配置
builder.Services.AddSqlSugarSonwFlakeSetup(builder.Configuration);
// SqlSugarCodeFirst设置
builder.Services.AddCodeFirstSetup(builder.Configuration, typeof(BaseModel));

//示例服务的注入
builder.Services.AddTransient<IQuestionService, QuestionService>();
builder.Services.AddTransient<IQuestionTypeService, QuestionTypeService>();

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