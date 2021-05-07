using FluentValidation;
using NerdStore.SharedKernel.Commands;
using System;

namespace NerdStore.Vendas.Application.Commands
{
    public class AdicionarItemPedidoCommand : Command
    {
        //tudo necessario para add
        public Guid ClienteId { get; private set; }

        public Guid ProdutoId { get; private set; }
        public string Nome { get; private set; }
        public int Quantidade { get; private set; }
        public decimal ValorUnitario { get; private set; }

        public AdicionarItemPedidoCommand(Guid clienteId, Guid produtoId, string nome, int quantidade, decimal valorUnitario)
        {
            ClienteId = clienteId;
            ProdutoId = produtoId;
            Nome = nome;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
        }

        public override bool IsValid()
        {
            return new AdicionarItemPedidoCommandValidation().Validate(this).IsValid;
        }
    }

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