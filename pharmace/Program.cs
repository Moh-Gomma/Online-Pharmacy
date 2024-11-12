using HUc.Helper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using pharmace;
using pharmace.Helper;
using pharmace.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<PharmacyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var cookiePolicyOptions = new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.None,
};

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

// Add EmailService here, before building the app
builder.Services.AddScoped<EmailService>();

var app = builder.Build();


// Configure DocumentSettings
DocumentSettings.Configure(app.Environment, app.Configuration);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// 
using (var scope = app.Services.CreateScope())
{
    var _context = scope.ServiceProvider.GetRequiredService<PharmacyContext>();

    var adminuser = new userpermations
    {
        Email = "adminelbazbigo@gmail.com",
        Password = CommonMethods.ConvertToEncrypt("51234@"),
        Username = "MainAdmin5",
        fname = "ElBaz",
        lname = "Pharmacy",
        Address = "Al-Mansoura",
        PhoneNumber = "01060062211",
        Role = "admin",
        IsVerified = true  
    };

    var user = new userpermations
    {
        Email = "mohamed_Gomaa@gmail.com",
        Password = CommonMethods.ConvertToEncrypt("123456789#"),
        Username = "User",
        Role = "user",
        IsVerified = true  
    };

    var MainAdminExist = _context.userpermations.Any(u => u.Username == adminuser.Username);
    var UserExist = _context.userpermations.Any(u => u.Username == user.Username);

    if (!MainAdminExist)
        _context.userpermations.Add(adminuser);
    if (!UserExist)
        _context.userpermations.Add(user);

    if (!UserExist || !MainAdminExist)
        _context.SaveChanges();
}

app.Run();