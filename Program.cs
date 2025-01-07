using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Models;

// نقطه شروع برای تنظیمات برنامه، سرویس‌ها، و کانفیگ‌ها
var builder = WebApplication.CreateBuilder(args);

// Registering the DbContext service with the connection string
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Identity services configuration
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Configure password complexity
builder.Services.Configure<IdentityOptions>(options =>
{
    // تنظیمات رمز عبور
    options.Password.RequireDigit = true; // حداقل یک عدد
    options.Password.RequiredLength = 8; 
    options.Password.RequireNonAlphanumeric = true; 
    options.Password.RequireUppercase = true; 
    options.Password.RequireLowercase = true;  
});
//پیکربندی کوکی‌ها
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";        // مسیر صفحه ورود
    options.LogoutPath = "/Account/Logout";      // مسیر صفحه خروج
    options.AccessDeniedPath = "/Account/AccessDenied"; // مسیر دسترسی غیرمجاز
    options.SlidingExpiration = true; // برای تمدید مدت اعتبار کوکی
    options.ExpireTimeSpan = TimeSpan.FromDays(30); // مدت اعتبار کوکی
});

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

// Add services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
//ایجاد اپلیکیشن
var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Enable CORS
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    //pattern: "{controller=Account}/{action=Login}/{id?}");
    pattern: "{controller=Todo}/{action=Index}/{id?}");
//pattern: "{controller=Account}/{action=Register}/{id?}");

app.MapRazorPages();
//شروع اجرای اپلیکیشن
app.Run();
