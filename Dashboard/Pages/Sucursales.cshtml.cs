using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Modelo.EFCore;
using Modelo.Entidades;

namespace Dashboard.Pages
{
    public class SucursalesModel : PageModel
    {
        private readonly Contexto _context; 

        public SucursalesModel(Contexto context)
        {
            _context = context; 
        }

        public IList<Sucursal> ListaSucursales { get; set; } // Lista para almacenar las sucursales filtradas

        // Propiedades para capturar los filtros ingresados en la interfaz
        [BindProperty(SupportsGet = true)]
        public string NombreFiltro { get; set; }

        [BindProperty(SupportsGet = true)]
        public string UbicacionFiltro { get; set; }

        [BindProperty(SupportsGet = true)]
        public string TelefonoFiltro { get; set; }

        public async Task OnGetAsync()
        {
            var query = _context.Sucursales
                                .Include(s => s.Ventas)  
                                .AsQueryable();

            if (!string.IsNullOrEmpty(NombreFiltro))
            {
                query = query.Where(s => s.Nombre.Contains(NombreFiltro));
            }

            if (!string.IsNullOrEmpty(UbicacionFiltro))
            {
                query = query.Where(s => s.Ubicacion.Contains(UbicacionFiltro));
            }

            if (!string.IsNullOrEmpty(TelefonoFiltro))
            {
                query = query.Where(s => s.Telefono.Contains(TelefonoFiltro));
            }

            ListaSucursales = await query.ToListAsync();
        }

    }
}
