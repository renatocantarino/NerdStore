using NerdStore.SharedKernel.Messages;
using System;

namespace NerdStore.Vendas.Application.Events
{
    public class AplicarVoucherPedidoEvent : Event
    {
        public Guid ClienteId { get; private set; }

        public Guid PedidoId { get; private set; }

        public Guid VoucherId { get; private set; }

        public AplicarVoucherPedidoEvent(Guid clienteId, Guid pedidoId, Guid voucherId)
        {
            ClienteId = clienteId;
            PedidoId = pedidoId;
            VoucherId = voucherId;
        }
    }
}