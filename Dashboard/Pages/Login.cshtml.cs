using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Modelo.EFCore;
using System.Security.Cryptography;
using System.Text;

namespace Dashboard.Pages
{
    public class LoginModel : PageModel
    {
        private readonly Contexto _context;

        public LoginModel(Contexto context)
        {
            _context = context;
        }


        
        [BindProperty]
        public string NombreUsuario { get; set; }

        [BindProperty]
        public string Contraseña { get; set; }

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(NombreUsuario) || string.IsNullOrEmpty(Contraseña))
            {
                ErrorMessage = "Por favor, completa todos los campos.";
                return Page();
            }


            // Busca en la base de datos un usuario que tenga el NombreUsuario que se ingresó.
            // Utiliza la función FirstOrDefaultAsync para obtener el primer usuario que coincida
            // con el nombre de usuario o retorna null si no encuentra ninguno
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.NombreUsuario == NombreUsuario);

            if (usuario == null || !VerificarContraseña(Contraseña, usuario.Contraseña))
            {
                ErrorMessage = "Nombre de usuario o contraseña incorrectos.";
                return Page();
            }

            // Si el usuario existe y la contraseña es correcta, se redirige al usuario a la página "Index".
            return Redirect("/Index"); // Redirige a la página Index.cshtml
        }

        private bool VerificarContraseña(string contraseñaIngresada, string contraseñaHashAlmacenada)
        {
            using (var sha256 = SHA256.Create())
            {
                var contraseñaIngresadaBytes = Encoding.UTF8.GetBytes(contraseñaIngresada);
                var contraseñaIngresadaHashBytes = sha256.ComputeHash(contraseñaIngresadaBytes);
                var contraseñaIngresadaHashString = Convert.ToBase64String(contraseñaIngresadaHashBytes);
                return contraseñaIngresadaHashString == contraseñaHashAlmacenada;
            }
        }
    }
}

