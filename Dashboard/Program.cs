using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Modelo.EFCore;
using Modelo.Entidades;
using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configura servicios de autenticaci�n
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login"; // Ruta a la p�gina de inicio de sesi�n
        options.LogoutPath = "/Logout"; // Ruta a la p�gina de cierre de sesi�n
    });

// Configurar servicios para el contenedor
builder.Services.AddDbContext<Contexto>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Agregamos soporte para Razor Pages
builder.Services.AddRazorPages();

var app = builder.Build();

// Configuraci�n del middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts(); // Activa HSTS para mejorar la seguridad en producci�n
}

app.UseHttpsRedirection(); // Fuerza el uso de HTTPS
app.UseStaticFiles(); // Habilita el uso de archivos est�ticos como im�genes, CSS, JS

app.UseRouting(); // Habilita el enrutamiento en la aplicaci�n

app.MapRazorPages(); // Mapea las p�ginas Razor a las rutas

// Redirigir a la p�gina de inicio de sesi�n
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
        var contrase�a = "login";
        var contrase�aEncriptada = EncriptarContrase�a(contrase�a);

        var usuario = new Usuario
        {
            NombreUsuario = "admin",
            Contrase�a = contrase�aEncriptada
        };

        context.Usuarios.Add(usuario);
        context.SaveChanges();
    }
}*/

app.Run(); // Ejecuta la aplicaci�n



// M�todo para encriptar la contrase�a
string EncriptarContrase�a(string contrase�a)
{
    // SHA256 produce un hash de 256 bits (32 bytes) que se suele representar
    using (var sha256 = SHA256.Create())
    {
        // Convierte la contrase�a ingresada en una secuencia de bytes usando UTF-8
        var contrase�aBytes = Encoding.UTF8.GetBytes(contrase�a);

        // Aplica el algoritmo SHA-256 a la secuencia de bytes de la contrase�a.
        var contrase�aHashBytes = sha256.ComputeHash(contrase�aBytes);

        // Convierte el arreglo de bytes del hash en una cadena codificada en Base64
        // Base64 es una representaci�n que usa 64 caracteres (letras, n�meros y algunos s�mbolos)
        // para representar datos binarios como una cadena de texto
        return Convert.ToBase64String(contrase�aHashBytes);
    }
}