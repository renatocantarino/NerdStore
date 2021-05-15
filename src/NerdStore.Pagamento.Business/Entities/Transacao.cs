using NerdStore.SharedKernel.DomainObjects;
using System;

namespace NerdStore.Pagamento.Business.Entities
{
    public class Transacao : Entity
    {
        public Guid PedidoId { get; set; }
        public Guid PagamentoId { get; set; }
        public decimal Total { get; set; }
        public StatusTransacao StatusTransacao { get; set; }

        //ef relacionamento
        public Payment Pagamento { get; set; }
    }
}