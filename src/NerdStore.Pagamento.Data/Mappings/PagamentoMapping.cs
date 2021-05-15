using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NerdStore.Pagamento.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Pagamento.Data.Mappings
{
    public class PagamentoMapping : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.NomeCartao)
                .IsRequired()
                .HasColumnType("varchar(250)");

            builder.Property(c => c.NumeroCartao)
                .IsRequired()
                .HasColumnType("varchar(16)");

            builder.Property(c => c.ExpiracaoCartao)
                .IsRequired()
                .HasColumnType("varchar(10)");

            builder.Property(c => c.CvvCartao)
                .IsRequired()
                .HasColumnType("varchar(4)");

            // 1 : 1 => Pagamento : Transacao
            builder.HasOne(c => c.Transacao)
                .WithOne(c => c.Pagamento);

            builder.ToTable("Pagamentos");
        }
    }
}