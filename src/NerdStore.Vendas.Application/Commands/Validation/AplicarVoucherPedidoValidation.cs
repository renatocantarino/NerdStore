using FluentValidation;
using System;

namespace NerdStore.Vendas.Application.Commands.Validation
{
    public class AplicarVoucherPedidoValidation : AbstractValidator<AplicarVoucherPedidoCommand>
    {
        public AplicarVoucherPedidoValidation()
        {
            RuleFor(i => i.ClienteId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id cliente invalido");

            RuleFor(i => i.Codigo)
               .NotEmpty()
               .NotNull()
               .WithMessage("Voucher invalido");
        }
    }
}