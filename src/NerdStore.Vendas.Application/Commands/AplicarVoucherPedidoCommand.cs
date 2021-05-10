using NerdStore.SharedKernel.Commands;
using NerdStore.Vendas.Application.Commands.Validation;
using System;

namespace NerdStore.Vendas.Application.Commands
{
    public class AplicarVoucherPedidoCommand : Command
    {
        public Guid ClienteId { get; private set; }
        public string Codigo { get; private set; }

        public AplicarVoucherPedidoCommand(Guid clienteId, string codigo)
        {
            ClienteId = clienteId;
            Codigo = codigo;
        }

        public override bool IsValid() => new AplicarVoucherPedidoValidation().Validate(this).IsValid;
    }
}