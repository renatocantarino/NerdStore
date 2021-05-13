using NerdStore.SharedKernel.DomainObjects.Dto;
using System;

namespace NerdStore.SharedKernel.Messages.Commom.IntegrationEvents
{
    public class PedidoIniciadoEvent : IntegrationEvent
    {
        public Guid ClienteId { get; private set; }
        public Guid PedidoId { get; private set; }
        public decimal Total { get; private set; }
        public string TitularCartao { get; private set; }
        public ListaProdutoPedido ListaProdutoPedido { get; private set; }
        public string NumeroCartao { get; private set; }
        public string ExpiracaoCartao { get; private set; }
        public string CvvCartao { get; private set; }

        public PedidoIniciadoEvent(Guid clienteId, Guid pedidoId, decimal total, string titularCartao, string numeroCartao,
                                   string expiracaoCartao, string cvvCartao, ListaProdutoPedido listaProdutoPedido)
        {
            AggregateId = pedidoId;
            ClienteId = clienteId;
            PedidoId = pedidoId;
            Total = total;
            TitularCartao = titularCartao;
            NumeroCartao = numeroCartao;
            ExpiracaoCartao = expiracaoCartao;
            CvvCartao = cvvCartao;
            ListaProdutoPedido = listaProdutoPedido;
        }
    }
}