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
        public string Contrase�a { get; set; }

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(NombreUsuario) || string.IsNullOrEmpty(Contrase�a))
            {
                ErrorMessage = "Por favor, completa todos los campos.";
                return Page();
            }


            // Busca en la base de datos un usuario que tenga el NombreUsuario que se ingres�.
            // Utiliza la funci�n FirstOrDefaultAsync para obtener el primer usuario que coincida
            // con el nombre de usuario o retorna null si no encuentra ninguno
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.NombreUsuario == NombreUsuario);

            if (usuario == null || !VerificarContrase�a(Contrase�a, usuario.Contrase�a))
            {
                ErrorMessage = "Nombre de usuario o contrase�a incorrectos.";
                return Page();
            }

            // Si el usuario existe y la contrase�a es correcta, se redirige al usuario a la p�gina "Index".
            return Redirect("/Index"); // Redirige a la p�gina Index.cshtml
        }

        private bool VerificarContrase�a(string contrase�aIngresada, string contrase�aHashAlmacenada)
        {
            using (var sha256 = SHA256.Create())
            {
                var contrase�aIngresadaBytes = Encoding.UTF8.GetBytes(contrase�aIngresada);
                var contrase�aIngresadaHashBytes = sha256.ComputeHash(contrase�aIngresadaBytes);
                var contrase�aIngresadaHashString = Convert.ToBase64String(contrase�aIngresadaHashBytes);
                return contrase�aIngresadaHashString == contrase�aHashAlmacenada;
            }
        }
    }
}

