using Microsoft.AspNetCore.Authentication.Cookies;
using News.Common;
using News.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<NewsDbContext>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<BytesEqualityComparer>();
builder.Services.AddScoped<PasswordHasherOptions>();
builder.Services.AddScoped<VerifyHashPassword>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

var cookiePolicyOptions = new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
};


var app = builder.Build();
app.UseCookiePolicy(cookiePolicyOptions);

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

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllerRoute(
//      name: "admin",
//      pattern: "admin/{controller=Home}/{action=Index}/{id?}"
//    );
//});



app.MapAreaControllerRoute(
    name: "admin",
    areaName: "admin",
    pattern: "admin/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);



app.Run();
