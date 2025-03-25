using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using TEATRO.Data;

var builder = WebApplication.CreateBuilder(args);

// ===============================
// Configuración de Servicios
// ===============================

// Configurar la conexión a la base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Configurar autenticación con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuarios/Login"; // Página de Login
    });

// Configurar sesiones
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Agregar controladores con vistas (MVC)
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ===============================
// Configuración del Middleware
// ===============================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// Registrar endpoints de los controladores API (por ejemplo, FacturasController)
app.MapControllers();

// ===============================
// Configuración de Rutas (MVC)
// ===============================
const string DefaultController = "Home";
const string DefaultAction = "Index";

// Ruta por defecto (Inicio)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=" + DefaultController + "}/{action=" + DefaultAction + "}/{id?}"
);

app.MapControllerRoute(
    name: "usuarios",
    pattern: "Usuarios/{action=Create}/{id?}",
    defaults: new { controller = "Usuarios", action = "Create" }
);

// Ruta del Panel de Administración (protegida por política de autorización)
app.MapControllerRoute(
    name: "admin",
    pattern: "AdministradorDashboard/{action=Index}/{id?}",
    defaults: new { controller = "AdministradorDashboard" }
).RequireAuthorization("AdminPolicy");

app.MapControllerRoute(
    name: "productos",
    pattern: "Productos/{action=Index}/{id?}",
    defaults: new { controller = "Productos" }
);

// Ruta para manejar errores personalizados
app.MapControllerRoute(
    name: "error",
    pattern: "Error/{statusCode}",
    defaults: new { controller = "Home", action = "Error" }
);

app.Run();


