using System;

namespace NerdStore.SharedKernel.Messages.Commom.IntegrationEvents.Pedido
{
    public class PedidoEstoqueRejeitadoEvent : IntegrationEvent
    {
        public Guid PedidoId { get; set; }
        public Guid ClienteId { get; set; }

        public PedidoEstoqueRejeitadoEvent(Guid pedidoId, Guid clienteId)
        {
            AggregateId = pedidoId;
            PedidoId = pedidoId;
            ClienteId = clienteId;
        }
    }
}