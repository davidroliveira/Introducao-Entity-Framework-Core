using CursoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CursoEFCore.Data.Configurations
{
    public class PedidoItemConfiguration : IEntityTypeConfiguration<PedidoItem>
    {
        public void Configure(EntityTypeBuilder<PedidoItem> builder)
        {
            builder.ToTable("PedidoItem");
            builder.HasKey(pedidoItem => pedidoItem.Id);
            builder.Property(pedidoItem => pedidoItem.Quantidade).HasDefaultValue(0).IsRequired();
            builder.Property(pedidoItem => pedidoItem.Valor).HasDefaultValue(0).IsRequired();
            builder.Property(pedidoItem => pedidoItem.Desconto).HasDefaultValue(0).IsRequired();
        }
    }
}
