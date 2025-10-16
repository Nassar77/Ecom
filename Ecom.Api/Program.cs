using Ecom.infrastructure;
using Ecom_Api.Middleware;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Scalar.AspNetCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(op =>
{
    op.AddPolicy("CORSPolicy", builder =>
    {
        builder.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("https://localhost:4200");
    });
});
builder.Services.AddMemoryCache();
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.infrastructureConfiguration(builder.Configuration);
//add maspster 
var mappConfig=TypeAdapterConfig.GlobalSettings;
mappConfig.Scan(Assembly.GetExecutingAssembly());
builder.Services.AddSingleton<IMapper>(new Mapper(mappConfig));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseCors("CORSPolicy");
app.UseMiddleware<ExceptionMiddleware>();

app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
