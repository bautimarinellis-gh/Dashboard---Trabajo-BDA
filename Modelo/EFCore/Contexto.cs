using Microsoft.EntityFrameworkCore;
using Modelo.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.EFCore
{
    public class Contexto:DbContext
    {
        public Contexto(DbContextOptions<Contexto> options)
            : base(options)
        {
             
        }

        public DbSet<Auto> Autos { get; set; }

        public DbSet<Sucursal> Sucursales { get; set; }

        public DbSet<Venta> Ventas { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DBDashboard;Integrated Security=True" +
                ";Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }
    }
}

