using CursoEFCore.Data.Configurations;
using CursoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CursoEFCore.Data
{
    public class ApplicationContext : DbContext
    {
        private static readonly ILoggerFactory _logger = LoggerFactory.Create(p => p.AddConsole());

        //public DbSet<Pedido> Pedido { get; set; }
        //public DbSet<Produto> Produto { get; set; }
        //public DbSet<Cliente> Cliente { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(_logger)
                          .EnableSensitiveDataLogging()
                          .UseSqlServer("Data Source=(localdb)\\mssqllocaldb;Initial Catalog=CursoEFCore;Integrated Security=True");
        }
                
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /* Metodo 1
            modelBuilder.ApplyConfiguration(new ClienteConfiguration());
            modelBuilder.ApplyConfiguration(new ProdutoConfiguration());
            modelBuilder.ApplyConfiguration(new PedidoConfiguration());
            modelBuilder.ApplyConfiguration(new PedidoItemConfiguration());
            */

            //Metodo 2
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
            
            MapearPropriedadesEsquecidas(modelBuilder);
        }

        private void MapearPropriedadesEsquecidas(ModelBuilder modelBuilder)
        {
            //RTTI
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                //Exemplo para tipo String evitar varchar(max)
                var properties = entity.GetProperties().Where(p => p.ClrType == typeof(string)); 

                foreach (var property in properties)
                {
                    if (string.IsNullOrEmpty(property.GetColumnType()) /*property.SetColumnType("VARCHAR(100)")*/ 
                        && !property.GetMaxLength().HasValue /*property.SetMaxLength(100);*/)
                    {
                        //property.SetMaxLength(100);
                        property.SetColumnType("VARCHAR(100)");
                    }
                }
            }
        }

    }
}
