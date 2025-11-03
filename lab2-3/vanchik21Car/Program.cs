using Microsoft.EntityFrameworkCore;
using vanchik21Car.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<VehicleDbContext>(opts => {
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:vanchik21CarConnection"]);
});

builder.Services.AddScoped<IVehicleRepository, EFVehicleRepository>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.MapDefaultControllerRoute();

SeedData.EnsurePopulated(app);

app.Run();
