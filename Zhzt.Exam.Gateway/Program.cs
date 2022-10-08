using Microsoft.AspNetCore.Http.Features;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Nacos;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add Ocelot Support
builder.Services.AddOcelot().AddNacosDiscovery();

// Add Nacos Support
builder.Host.ConfigureAppConfiguration((context, builder) =>
{
    var c = builder.Build();
    builder.AddNacosV2Configuration(c.GetSection("NacosConfig"));
});

builder.Services.Configure<FormOptions>(x =>
{
    x.ValueLengthLimit = int.MaxValue;
    x.MultipartBodyLengthLimit = int.MaxValue;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/questionlib/swagger/v1/swagger.json", "QuestionLib API");
        c.SwaggerEndpoint("/paperlib/swagger/v1/swagger.json", "PaperLib API");
        c.SwaggerEndpoint("/authcenter/swagger/v1/swagger.json", "UserLib API");
        c.SwaggerEndpoint("/microclasslib/swagger/v1/swagger.json", "MicroClassLib API");
        c.SwaggerEndpoint("/documentlib/swagger/v1/swagger.json", "DocumentLib API");
    });
}

app.UseOcelot();

app.UseAuthorization();

app.MapControllers();

app.Run();
