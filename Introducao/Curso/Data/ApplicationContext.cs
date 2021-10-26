using CursoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CursoEFCore.Data
{
    public class ApplicationContext : DbContext
    {
        //Formato de configuração 1
        //public DbSet<Pedido> Pedido { get; set; }
        //public DbSet<PedidoItem> PedidoItem { get; set; }
        //public DbSet<Cliente> Cliente { get; set; }
        //public DbSet<Produto> Produto { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\mssqllocaldb;Initial Catalog=CursoEFCore;Integrated Security=True");
        }

        //Formato de configuração 2
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>(entidade =>
            {
                entidade.ToTable("Cliente");
                entidade.HasKey(cliente => cliente.Id);
                entidade.Property(cliente => cliente.Nome).HasColumnType("VARCHAR(80)").IsRequired();
                entidade.Property(cliente => cliente.Telefone).HasColumnType("CHAR(11)");
                entidade.Property(cliente => cliente.CEP).HasColumnType("CHAR(8)").IsRequired();
                entidade.Property(cliente => cliente.Estado).HasColumnType("CHAR(2)").IsRequired();
                entidade.Property(cliente => cliente.Cidade).HasMaxLength(60).IsRequired();

                entidade.HasIndex(index => index.Telefone).HasName("IDX_CLIENTE_TELEFONE");
            });

            modelBuilder.Entity<Produto>(entidade =>
            {
                entidade.ToTable("Produto");
                entidade.HasKey(produto => produto.Id);
                entidade.Property(produto => produto.CodigoBarras).HasColumnType("VARCHAR(14)").IsRequired();
                entidade.Property(produto => produto.Descricao).HasColumnType("VARCHAR(60)");
                entidade.Property(produto => produto.Valor).IsRequired();
                entidade.Property(produto => produto.TipoProduto).HasConversion<string>();
            });

            modelBuilder.Entity<Pedido>(entidade =>
            {
                entidade.ToTable("Pedido");
                entidade.HasKey(pedido => pedido.Id);
                entidade.Property(pedido => pedido.IniciadoEm).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();
                entidade.Property(pedido => pedido.Status).HasConversion<string>();
                entidade.Property(pedido => pedido.TipoFrete).HasConversion<int>();
                entidade.Property(pedido => pedido.Observacao).HasColumnType("VARCHAR(512)");

                entidade.HasMany(pedido => pedido.Itens)
                      .WithOne(pedido => pedido.Pedido)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PedidoItem>(entidade =>
            {
                entidade.ToTable("PedidoItem");
                entidade.HasKey(pedidoItem => pedidoItem.Id);
                entidade.Property(pedidoItem => pedidoItem.Quantidade).HasDefaultValue(0).IsRequired();
                entidade.Property(pedidoItem => pedidoItem.Valor).HasDefaultValue(0).IsRequired();
                entidade.Property(pedidoItem => pedidoItem.Desconto).HasDefaultValue(0).IsRequired();
            });
        }
    }
}
