using Npgsql;
using System.Text;
using System.Data;
using ClinicApplication.Helpers;
using ClinicApplication.Models;
using ClinicApplication.Services;
using ClinicApplication.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


// Đăng ký dịch vụ
builder.Services.AddSingleton<DatabaseHelper>();
builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.Configure<FileSettingOptions>(builder.Configuration.GetSection("FileSettings"));

// Cấu hình Swagger + hỗ trợ JWT Bearer
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Clinic API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nhập token theo định dạng: Bearer {token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Cấu hình xác thực JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
        )
    };
});

// Đăng ký DI container
builder.Services.AddScoped<BenhNhanService>();
builder.Services.AddScoped<PhieuKhamService>();
builder.Services.AddScoped<PhieuKhamHinhAnhService>();
builder.Services.AddScoped<RegisterService>();
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<BenhNhanRepository>();
builder.Services.AddScoped<PhieuKhamRepository>();
builder.Services.AddScoped<PhieukhamHinhAnhRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<PasswordHasher<User>>();

// Tạo ứng dụng
var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();             // 1. Routing trước
app.UseAuthentication();     // 2. Xác thực JWT
app.UseAuthorization();      // 3. Xác quyền theo [Authorize]
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // 4. Map controllers
});

app.Run();
