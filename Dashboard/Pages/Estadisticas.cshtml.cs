using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Modelo.EFCore;

namespace Dashboard.Pages
{
    public class EstadisticasModel : PageModel
    {
        private readonly Contexto _context;

        public EstadisticasModel(Contexto context)
        {
            _context = context;
        }

        // Propiedades para almacenar la cantidad de ventas por sucursal
        public int VentasSucursalCentral { get; set; }
        public int VentasSucursalNorte { get; set; }
        public int VentasSucursalSur { get; set; }
        public int VentasSucursalOeste { get; set; }
        public int VentasSucursalEste { get; set; }

        public async Task OnGetAsync()
        {
            // Obtener las ventas por sucursal
            VentasSucursalCentral = await _context.Ventas.CountAsync(v => v.Sucursal.Id== 1);
            VentasSucursalNorte = await _context.Ventas.CountAsync(v => v.Sucursal.Id == 2);
            VentasSucursalSur = await _context.Ventas.CountAsync(v => v.Sucursal.Id== 3);
            VentasSucursalOeste = await _context.Ventas.CountAsync(v => v.Sucursal.Id == 4);
            VentasSucursalEste = await _context.Ventas.CountAsync(v => v.Sucursal.Id == 5);
        }
    }
}
