using CdApp.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Stöd för MVC
builder.Services.AddControllersWithViews();

// Databashanterare och anslutningssträng
builder.Services.AddDbContext<CdContext>(options =>
options.UseSqlite(builder.Configuration.GetConnectionString("DefaultDbString")));

builder.Services.AddDistributedMemoryCache();

// Inställningar för sessioner
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(600);
    options.Cookie.HttpOnly = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

// Stöd för statiska filer
app.UseStaticFiles();

app.UseCookiePolicy();

// Aktiverar sessionsvariabler
app.UseSession();

app.UseRouting();

// Routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Cd}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "Artist",
    pattern: "Artist/{*ArtistId}",
    defaults: new { controller = "Artist", action = "Index" }
);

app.UseAuthorization();

app.MapRazorPages();

app.Run();
