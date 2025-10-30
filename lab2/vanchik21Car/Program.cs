using Microsoft.EntityFrameworkCore;
using vanchik21Car.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<VehicleDbContext>(opts => {
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:vanchik21CarConnection"]);
});

builder.Services.AddScoped<IVehicleRepository, EFVehicleRepository>();

var app = builder.Build();

app.UseStaticFiles();

app.MapDefaultControllerRoute();

SeedData.EnsurePopulated(app);

app.Run();
