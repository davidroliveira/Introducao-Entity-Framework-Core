using CursoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CursoEFCore.Data.Configurations
{
    public class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable("Produto");
            builder.HasKey(produto => produto.Id);
            builder.Property(produto => produto.CodigoBarras).HasColumnType("VARCHAR(14)").IsRequired();
            builder.Property(produto => produto.Descricao).HasColumnType("VARCHAR(60)");
            builder.Property(produto => produto.Valor).IsRequired();
            builder.Property(produto => produto.TipoProduto).HasConversion<string>();
        }
    }
}
