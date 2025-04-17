using Negocios;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

QuestPDF.Settings.License = LicenseType.Community; // <-- aquí se pone

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession(); // Habilita sesiones
builder.Services.AddTransient<CorreoService>();

var app = builder.Build();
app.UseSession(); // Activa el uso de sesiones


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
