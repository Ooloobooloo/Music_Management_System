using Microsoft.EntityFrameworkCore;
using Music_Management_System.Data;
using Music_Management_System.Helpers;
using Music_Management_System.Interfaces;
using Music_Management_System.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

// Register services
builder.Services.AddScoped<IMp3Service, Mp3Service>();
builder.Services.AddScoped<IPhotoService, PhotoService>();

var app = builder.Build();

// ==================== DATABASE SEEDING ====================
if (app.Environment.IsDevelopment())
{
    Console.WriteLine("🌱 Starting database seeding in Development mode...");
    DbInitializer.Seed(app);
}
// ========================================================

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Song}/{action=Index}/{id?}");

app.Run();