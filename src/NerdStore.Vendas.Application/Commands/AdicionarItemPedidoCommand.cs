using FluentValidation;
using NerdStore.SharedKernel.Commands;
using NerdStore.Vendas.Application.Commands.Validation;
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

        public override bool IsValid() => new AdicionarItemPedidoCommandValidation().Validate(this).IsValid;
    }
}