using FluentValidation;
using System;

namespace NerdStore.Vendas.Application.Commands.Validation
{
    public class AtualizarItemPedidoValidation : AbstractValidator<AtualizarItemPedidoCommand>
    {
        public AtualizarItemPedidoValidation()
        {
            RuleFor(i => i.ClienteId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id cliente invalido");

            RuleFor(i => i.ProdutoId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id Produto invalido");

            RuleFor(i => i.Quantidade)
                .GreaterThan(0)
                    .WithMessage("Quantidade minima é 0")
                .LessThan(15)
                    .WithMessage("Quantidade maxima é 15");
        }
    }
}