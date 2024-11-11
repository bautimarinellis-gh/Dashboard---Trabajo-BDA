using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Entidades
{
    public class Auto
    {
        public int Id { get; set; }

        public string Marca { get; set; }

        public string Modelo { get; set; }

        public int Año { get; set; }

        public decimal Precio { get; set; }

        public Sucursal Sucursal { get; set; }

        public override string ToString()
        {
            return $"{Modelo} )";
        }
    }
}
