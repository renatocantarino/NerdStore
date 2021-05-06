using NerdStore.SharedKernel.DomainObjects;
using System;

namespace NerdStore.Vendas.Domain.Entities
{
    public class PedidoItem : Entity
    {
        public Guid PedidoId { get; private set; }
        public Guid ProdutoId { get; private set; }
        public string Nome { get; private set; }
        public int Quantidade { get; private set; }
        public decimal ValorUnitario { get; private set; }
        public virtual Pedido Pedido { get; private set; }

        public PedidoItem(Guid produtoId, string nome, int quantidade, decimal valorUnitario)
        {
            this.ProdutoId = produtoId;
            this.Nome = nome;
            this.Quantidade = quantidade;
            this.ValorUnitario = valorUnitario;
        }

        protected PedidoItem()
        {
        }

        internal void AssociarPedido(Guid pedidoId) => PedidoId = pedidoId;

        public decimal Calcular() => Quantidade * ValorUnitario;

        internal void AdicionarUnidades(int unidades) => Quantidade += unidades;

        internal void AtualizarUnidades(int unidades) => Quantidade = unidades;

        public bool IsValid() => true;
    }
}