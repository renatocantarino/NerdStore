using NerdStore.SharedKernel.Data.Repository;
using NerdStore.Vendas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Domain.Repositories
{
    public interface IPedidoRepository : IRepository<Pedido>
    {
        Task<Pedido> ObterPorId(Guid id);

        Task<IReadOnlyCollection<Pedido>> ObterListaPorCliente(Guid clienteId);

        Task<Pedido> ObterPedidoRascunhoPorCliente(Guid clienteId);

        void Adicionar(Pedido pedido);

        void Atualizar(Pedido pedido);

        Task<PedidoItem> ObterItemPorId(Guid id);

        Task<PedidoItem> ObterItemPorPedido(Guid pedidoId, Guid produtoId);

        void AdicionarItem(PedidoItem pedidoItem);

        void AtualizarItem(PedidoItem pedidoItem);

        void RemoverItem(PedidoItem pedidoItem);

        Task<Voucher> ObterVoucherPorCodigo(string codigo);
    }
}