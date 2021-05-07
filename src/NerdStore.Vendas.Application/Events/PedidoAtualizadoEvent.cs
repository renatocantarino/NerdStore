using NerdStore.SharedKernel.Messages;
using System;

namespace NerdStore.Vendas.Application.Events
{
    public class PedidoAtualizadoEvent : Event
    {
        public Guid ClienteId { get; private set; }

        public Guid PedidoId { get; private set; }

        public decimal ValorTotoal { get; private set; }

        public PedidoAtualizadoEvent(Guid clienteId, Guid pedidoId, decimal valorTotoal)
        {
            AggregateId = pedidoId;
            ClienteId = clienteId;
            PedidoId = pedidoId;
            ValorTotoal = valorTotoal;
        }
    }
}