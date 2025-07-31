using Common.Utilities.Utils;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SSO_Api.Data;
using SSO_Api.Models;
using SSO_Api.Repositories.MyAdgEF;
using SSO_Api.Repositories.RepositorySystemEF;
using System.Text;
using WebApi.Helpers;
using System;



var builder = WebApplication.CreateBuilder(args);

// 1) Đọc connection-string
//var conn = builder.Configuration.GetConnectionString("default");
var conn = Environment.GetEnvironmentVariable("CONNECTION_STRING");

// 2) Đăng ký DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(conn)
);

//var connectionString = builder.Configuration.GetConnectionString("default");

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
}); ;

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(conn));

// 1) Đăng ký IHttpContextAccessor nếu bạn cần dùng HttpContext
builder.Services.AddHttpContextAccessor();

// 2) Đăng ký WebHelpers để nó có thể nhận IConfiguration và IHttpContextAccessor
builder.Services.AddSingleton<WebHelpers>();

//cho phép nhận json không cần đủ parameter
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddScoped<ISystemManagement, SystemManagement>();
builder.Services.AddScoped<IDeXuatManagement, DeXuatManagement>();

// ✳️ Đăng ký cấu hình appsettings (bind vào class JWTAppSetting)
builder.Services.Configure<JWTAppSetting>(builder.Configuration.GetSection("JWTAppSetting"));
// ✳️ Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.WithOrigins("http://localhost:4200",
                            "https://your-app-name.railway.app")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed(_ => true);
    });
});


//builder.Services.AddIdentity<AppUser, IdentityRole>(
//    options =>
//    {
//        options.Password.RequiredUniqueChars = 0;
//        options.Password.RequireUppercase = false;
//        options.Password.RequiredLength = 8;
//        options.Password.RequireNonAlphanumeric = false;
//        options.Password.RequireLowercase = false;
//    })
//    .AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

//var secretKey = builder.Configuration["Jwt:Key"] ?? "KSAJEF83URIWUHAE834JFKIQSEJFBAKN";
//var issuer = builder.Configuration["Jwt:Issuer"] ?? "your-sso-server";
//var audience = builder.Configuration["Jwt:Audience"] ?? "your-client-apps";

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,

//            ValidIssuer = "your-sso-server",
//            ValidAudience = "your-client-apps",
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
//        };
//    });

//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(options =>
//    {
//        options.Cookie.Name = ".YourName.Auth";
//        //options.Cookie.Domain = ".yourdomain.com";
//        options.LoginPath = "/Account/Login";
//        options.LogoutPath = "/Account/Logout";
//        options.Cookie.HttpOnly = true;
//        options.Cookie.SameSite = SameSiteMode.Lax;
//        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
//    });

var app = builder.Build();

// ✳️ Global CORS policy
app.UseCors("CorsPolicy");
// ✳️ Custom JWT middleware
app.UseMiddleware<JwtMiddleware>();

app.MapControllers();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
