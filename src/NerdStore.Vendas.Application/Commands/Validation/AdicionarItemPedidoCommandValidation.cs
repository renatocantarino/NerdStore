using FluentValidation;
using System;

namespace NerdStore.Vendas.Application.Commands.Validation
{
    public class AdicionarItemPedidoCommandValidation : AbstractValidator<AdicionarItemPedidoCommand>
    {
        public AdicionarItemPedidoCommandValidation()
        {
            RuleFor(p => p.ClienteId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Id do cliente invalido");

            RuleFor(p => p.ProdutoId)
                   .NotEqual(Guid.Empty)
                   .WithMessage("Id do cliente invalido");

            RuleFor(p => p.Nome)
                  .NotEmpty()
                  .WithMessage("Nome do produto invalido");

            RuleFor(p => p.Quantidade)
                  .GreaterThan(0)
                  .WithMessage("Quantidade minima é 1");

            RuleFor(p => p.Quantidade)
                .LessThan(15)
                .WithMessage("Quantidade maxima é 15");

            RuleFor(p => p.ValorUnitario)
                 .GreaterThan(0)
                 .WithMessage("O valor unitario precisa ser maior que R$ 0,00");
        }
    }
}