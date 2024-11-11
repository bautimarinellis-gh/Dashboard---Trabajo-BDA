using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Entidades
{
    public class Venta
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public decimal PrecioFinal { get; set; }
        public Auto Auto { get; set; }
        public Sucursal Sucursal { get; set; }
    }
}
