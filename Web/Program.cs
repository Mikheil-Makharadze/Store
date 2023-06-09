using Microsoft.AspNetCore.Authentication.Cookies;
using Web.ExceptionFilter;
using Web.Mapping;
using Web.Services;
using Web.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<ApiExceptionFilterAttribute>();
});
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddHttpClient<IProductService, ProductService>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddHttpClient<IProducerService, ProducerService>();
builder.Services.AddScoped<IProducerService, ProducerService>();

builder.Services.AddHttpClient<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddHttpClient<IAccountService, AccountService>();
builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddAuthentication
    (options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = "oidc";
    })
              .AddCookie(options =>
              {
                  options.Cookie.HttpOnly = true;
                  options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                  options.LoginPath = "/Auth/Login";
                  options.AccessDeniedPath = "/Auth/AccessDenied";
                  options.SlidingExpiration = true;
              });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));
    options.AddPolicy("AdminOrEmployee", policy =>
        policy.RequireRole("admin", "employee"));
});
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    //if(app.Environment.IsDevelopment())
    //{
    //    app.UseExceptionHandler("/Error-development");
    //}
    //else
    //{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.

    app.UseHsts();
}
//app.UseStatusCodePagesWithReExecute

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Index}");

app.Run();
