using ERP_API.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy
            .WithOrigins(
                "http://localhost:5173",
                "http://localhost:5174",
                "http://localhost:3000"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
    );
});


builder.Services.AddDbContext<ErpDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("MySqlConnection"),
        new MySqlServerVersion(new Version(9, 4, 0))
    ));


builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<ErpDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllers()
.AddNewtonsoftJson(options =>
 options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
   .AddCookie(options =>
   {
       options.LoginPath = "/auth/login";           // đường dẫn khi chưa đăng nhập
       options.LogoutPath = "/auth/logout";         // đường dẫn đăng xuất
       options.AccessDeniedPath = "/auth/denied";   // khi không có quyền truy cập
       options.Cookie.Name = "ERPAuthCookie";       // tên cookie
       options.Cookie.HttpOnly = true;              // bảo mật cookie
       options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // chỉ dùng HTTPS
       options.Cookie.SameSite = SameSiteMode.None; // cho phép từ frontend khác domain (nếu SPA)
       options.ExpireTimeSpan = TimeSpan.FromHours(3);          // thời gian sống
       options.SlidingExpiration = true;           // tự reset thời gian khi có hoạt động
   });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowReactApp");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
