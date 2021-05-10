using FluentValidation;
using FluentValidation.Results;
using NerdStore.SharedKernel.DomainObjects;
using NerdStore.Vendas.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace NerdStore.Vendas.Domain.Entities
{
    public class Voucher : Entity
    {
        public string Codigo { get; private set; }
        public decimal? Percentual { get; private set; }
        public decimal? ValorDesconto { get; private set; }
        public int Quantidade { get; private set; }
        public TipoDescontoVoucher TipoDescontoVoucher { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public DateTime? DataUtilizacao { get; private set; }
        public DateTime DataValidade { get; private set; }
        public bool Ativo { get; private set; }
        public bool Utilizado { get; private set; }

        // EF Rel.
        public ICollection<Pedido> Pedidos { get; set; }

        internal ValidationResult ehAplicavel() => new VoucherAplicavelValidation().Validate(this);
    }

    public class VoucherAplicavelValidation : AbstractValidator<Voucher>
    {
        public VoucherAplicavelValidation()
        {
            RuleFor(v => v.DataValidade)
                .Must(DataVencimentoSuperiorAtual)
                .WithMessage("Voucher expirado");

            RuleFor(v => v.Ativo)
                .Equal(true)
                .WithMessage("Voucher inativo");

            RuleFor(v => v.Utilizado)
                .Equal(false)
                .WithMessage("Voucher já utilizado");

            RuleFor(v => v.Quantidade)
               .GreaterThan(0)
               .WithMessage("Voucher não disponivel");
        }

        private bool DataVencimentoSuperiorAtual(DateTime dataValidade) => dataValidade > DateTime.Now;
    }
}