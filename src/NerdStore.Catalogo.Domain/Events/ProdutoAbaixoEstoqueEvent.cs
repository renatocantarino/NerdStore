using NerdStore.SharedKernel.DomainEvents;
using System;

namespace NerdStore.Catalogo.Domain.Events
{
    public class ProdutoAbaixoEstoqueEvent : Event
    {
        public int QuantidadeRestante { get; private set; }

        public ProdutoAbaixoEstoqueEvent(Guid agregatteId, int quantidadeRestante) : base(agregatteId)
        {
            this.QuantidadeRestante = quantidadeRestante;
        }
    }
}