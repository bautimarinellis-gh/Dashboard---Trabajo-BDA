using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Modelo.EFCore;
using Modelo.Entidades;
using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configura servicios de autenticación
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login"; // Ruta a la página de inicio de sesión
        options.LogoutPath = "/Logout"; // Ruta a la página de cierre de sesión
    });

// Configurar servicios para el contenedor
builder.Services.AddDbContext<Contexto>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Agregamos soporte para Razor Pages
builder.Services.AddRazorPages();

var app = builder.Build();

// Configuración del middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts(); // Activa HSTS para mejorar la seguridad en producción
}

app.UseHttpsRedirection(); // Fuerza el uso de HTTPS
app.UseStaticFiles(); // Habilita el uso de archivos estáticos como imágenes, CSS, JS

app.UseRouting(); // Habilita el enrutamiento en la aplicación

app.MapRazorPages(); // Mapea las páginas Razor a las rutas

// Redirigir a la página de inicio de sesión
app.MapGet("/", async context =>
{
    context.Response.Redirect("/Login");
});

// Crear el usuario admin
/*using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<Contexto>();

    // Verificar si el usuario admin ya existe
    if (!context.Usuarios.Any(u => u.NombreUsuario == "admin"))
    {
        var contraseña = "login";
        var contraseñaEncriptada = EncriptarContraseña(contraseña);

        var usuario = new Usuario
        {
            NombreUsuario = "admin",
            Contraseña = contraseñaEncriptada
        };

        context.Usuarios.Add(usuario);
        context.SaveChanges();
    }
}*/

app.Run(); // Ejecuta la aplicación



// Método para encriptar la contraseña
string EncriptarContraseña(string contraseña)
{
    // SHA256 produce un hash de 256 bits (32 bytes) que se suele representar
    using (var sha256 = SHA256.Create())
    {
        // Convierte la contraseña ingresada en una secuencia de bytes usando UTF-8
        var contraseñaBytes = Encoding.UTF8.GetBytes(contraseña);

        // Aplica el algoritmo SHA-256 a la secuencia de bytes de la contraseña.
        var contraseñaHashBytes = sha256.ComputeHash(contraseñaBytes);

        // Convierte el arreglo de bytes del hash en una cadena codificada en Base64
        // Base64 es una representación que usa 64 caracteres (letras, números y algunos símbolos)
        // para representar datos binarios como una cadena de texto
        return Convert.ToBase64String(contraseñaHashBytes);
    }
}