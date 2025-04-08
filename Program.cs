using Npgsql;
using System.Data;
using ClinicApplication.Helpers;
using Dapper.FastCrud;
using ClinicApplication.Models;
using ClinicApplication.Services;
using ClinicApplication.Repositories;


var builder = WebApplication.CreateBuilder(args);

OrmConfiguration.DefaultDialect = SqlDialect.PostgreSql;
builder.Services.AddSingleton<DatabaseHelper>();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.Configure<FileSettingOptions>(builder.Configuration.GetSection("FileSettings"));

builder.Services.AddScoped<BenhNhanService>();
builder.Services.AddScoped<PhieuKhamService>();
builder.Services.AddScoped<PhieuKhamHinhAnhService>();

builder.Services.AddScoped<BenhNhanRepository>();
builder.Services.AddScoped<PhieuKhamRepository>();
builder.Services.AddScoped<PhieukhamHinhAnhRepository>();

var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoint => endpoint.MapControllers());
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
    
app.Run();