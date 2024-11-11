using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Modelo.EFCore;
using Modelo.Entidades;

namespace Dashboard.Pages
{
    public class VentasModel : PageModel
    {
        private readonly Contexto _context;

        public VentasModel(Contexto context)
        {
            _context = context;
        }

        public IList<Venta> ListaVentas { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? FiltroFecha { get; set; }

        [BindProperty(SupportsGet = true)]
        public string FiltroPrecioFinal { get; set; }

        [BindProperty(SupportsGet = true)]
        public string FiltroAuto { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? SucursalId { get; set; } // Para recibir el id de la sucursal

        public decimal MontoTotal { get; set; } // Nueva propiedad para almacenar el monto total de la sucursal

        public async Task OnGetAsync()
        {
            var query = _context.Ventas
                .Include(v => v.Auto)
                .Include(v => v.Sucursal)
                .AsQueryable();

            // Filtrar por sucursal
            if (SucursalId.HasValue)
            {
                query = query.Where(v => v.Sucursal.Id == SucursalId.Value);
            }

            // Aplicar otros filtros
            if (FiltroFecha.HasValue)
            {
                query = query.Where(v => v.Fecha.Date == FiltroFecha.Value.Date);
            }

            if (!string.IsNullOrEmpty(FiltroPrecioFinal) && decimal.TryParse(FiltroPrecioFinal, out var precioFinal))
            {
                query = query.Where(v => v.PrecioFinal == precioFinal);
            }

            if (!string.IsNullOrEmpty(FiltroAuto))
            {
                query = query.Where(v => v.Auto.Modelo.Contains(FiltroAuto));
            }

            ListaVentas = await query.ToListAsync();

            // Calcular el monto total sumando las ventas filtradas
            MontoTotal = ListaVentas.Sum(v => v.PrecioFinal);
        }
    }
}
