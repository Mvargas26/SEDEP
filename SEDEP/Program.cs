using Microsoft.AspNetCore.Authentication.Cookies;
using Negocios;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

QuestPDF.Settings.License = LicenseType.Community; // <-- aquí se pone

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession(); // Habilita sesiones
builder.Services.AddTransient<CorreoService>();


//Servicio para usar la auntentificacion de mvc
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
{
    option.LoginPath = "/Acceso/validarUser";
    option.ExpireTimeSpan = TimeSpan.FromMinutes(5);
    option.AccessDeniedPath = "/Auth/AccessDenied";
});

var app = builder.Build();
app.UseSession(); // Activa el uso de sesiones


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Auth/AccessDenied");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
