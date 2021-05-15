using System;
using System.Collections.Generic;

namespace NerdStore.Pagamento.Business.Dtos
{
    public class Pedido
    {
        public Guid ClienteId { get; set; }
        public Guid Id { get; set; }
        public List<Produto> Produtos { get; set; }
        public decimal Valor { get; set; }
    }
}