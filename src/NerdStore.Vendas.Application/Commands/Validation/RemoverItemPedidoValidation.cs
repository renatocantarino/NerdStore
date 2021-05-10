using FluentValidation;
using System;

namespace NerdStore.Vendas.Application.Commands.Validation
{
    public class RemoverItemPedidoValidation : AbstractValidator<RemoverItemPedidoCommand>
    {
        public RemoverItemPedidoValidation()
        {
            RuleFor(i => i.ClienteId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id cliente invalido");

            RuleFor(i => i.ProdutoId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id Produto invalido");
        }
    }
}