using NerdStore.SharedKernel.Commands;
using NerdStore.Vendas.Application.Commands.Validation;
using System;

namespace NerdStore.Vendas.Application.Commands
{
    public class AtualizarItemPedidoCommand : Command
    {
        //entender a intenção de negocio
        public Guid ClienteId { get; private set; }

        public Guid ProdutoId { get; private set; }

        public int Quantidade { get; private set; }

        public AtualizarItemPedidoCommand(Guid clienteId, Guid produtoId, int quantidade)
        {
            ClienteId = clienteId;
            ProdutoId = produtoId;
            Quantidade = quantidade;
        }

        public override bool IsValid() => new AtualizarItemPedidoValidation().Validate(this).IsValid;
    }
}