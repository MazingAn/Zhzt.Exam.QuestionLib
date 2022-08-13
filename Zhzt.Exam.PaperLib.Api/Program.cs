using MongoDb.Extensions.Options;
using Nacos.AspNetCore.V2;
using Zhzt.Exam.PaperLib.DomainInterface;
using Zhzt.Exam.PaperLib.DomainService;

var builder = WebApplication.CreateBuilder(args);

// Add Nacos Support to load configuration from nacos server
builder.Host.ConfigureAppConfiguration((context, builder) =>
{
    var c = builder.Build();
    builder.AddNacosV2Configuration(c.GetSection("NacosConfig"));
});

// Add MongoDb Configuration
builder.Services.AddMongoDbConfig(builder.Configuration);
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IPaperService, PaperService>();

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
