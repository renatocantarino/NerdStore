using System;
using System.Collections.Generic;

namespace NerdStore.SharedKernel.DomainObjects.Dto
{
    public class ListaProdutoPedido
    {
        public Guid Id { get; set; }
        public ICollection<Item> Itens { get; set; }
    }
}