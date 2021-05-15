using NerdStore.SharedKernel.DomainObjects;
using System;

namespace NerdStore.Pagamento.Business.Entities
{
    public class Payment : Entity, IAggregateRoot
    {
        public Guid PedidoId { get; set; }
        public string Status { get; set; }
        public decimal Valor { get; set; }
        public string NomeCartao { get; set; }
        public string NumeroCartao { get; set; }
        public string ExpericaoCartao { get; set; }
        public string CvvCartao { get; set; }

        //ef relacionamento
        public Transacao Transacao { get; set; }
    }
}