using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Modelo.EFCore;
using Modelo.Entidades;

namespace Dashboard.Pages
{
    public class AutosModel : PageModel
    {
        private readonly Contexto _context; 

        public AutosModel(Contexto context)
        {
            _context = context; 
        }

        public IList<Auto> ListaAutos { get; set; } // Lista para almacenar los autos filtrados

        // Propiedades para capturar los valores de los filtros desde la interfaz
        [BindProperty(SupportsGet = true)]
        public string MarcaFiltro { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ModeloFiltro { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? AñoFiltro { get; set; }

        [BindProperty(SupportsGet = true)]
        public decimal? PrecioFiltro { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SucursalFiltro { get; set; }


        // Método que se ejecuta al cargar la página para obtener los autos filtrados
        public async Task OnGetAsync()
        {
            // Inicia una consulta base incluyendo la relación con Sucursal
            var query = _context.Autos.Include(a => a.Sucursal).AsQueryable();

            // Filtra por Marca si se ingresó un valor
            if (!string.IsNullOrEmpty(MarcaFiltro))
            {
                query = query.Where(a => a.Marca.Contains(MarcaFiltro));
            }

            // Filtra por Modelo si se ingresó un valor
            if (!string.IsNullOrEmpty(ModeloFiltro))
            {
                query = query.Where(a => a.Modelo.Contains(ModeloFiltro));
            }

            // Filtra por Año si se ingresó un valor
            if (AñoFiltro.HasValue)
            {
                query = query.Where(a => a.Año == AñoFiltro.Value);
            }

            // Filtra por Precio máximo si se ingresó un valor
            if (PrecioFiltro.HasValue)
            {
                query = query.Where(a => a.Precio <= PrecioFiltro.Value);
            }

            // Filtra por Sucursal si se ingresó un valor
            if (!string.IsNullOrEmpty(SucursalFiltro))
            {
                query = query.Where(a => a.Sucursal.Nombre.Contains(SucursalFiltro));
            }

            // Ejecuta la consulta y almacena los resultados en ListaAutos
            ListaAutos = await query.ToListAsync();
        }
    }
}
