using Microsoft.EntityFrameworkCore;
using NerdStore.SharedKernel.Data.Repository;
using NerdStore.Vendas.Data.Contexts;
using NerdStore.Vendas.Domain.Entities;
using NerdStore.Vendas.Domain.Repositories;
using NerdStore.Vendas.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Data.Repository
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly VendasContext _context;

        public PedidoRepository(VendasContext context) => _context = context;

        public IUnitOfWork UnitOfWork => _context;

        public void Adicionar(Pedido pedido) => _context.Pedidos.Add(pedido);

        public void AdicionarItem(PedidoItem pedidoItem) => _context.PedidoItems.Add(pedidoItem);

        public void Atualizar(Pedido pedido) => _context.Pedidos.Update(pedido);

        public void AtualizarItem(PedidoItem pedidoItem) => _context.PedidoItems.Update(pedidoItem);

        public async Task<PedidoItem> ObterItemPorId(Guid id) => await _context.PedidoItems.FindAsync(id);

        public async Task<PedidoItem> ObterItemPorPedido(Guid pedidoId, Guid produtoId)
            => await _context.PedidoItems.FirstOrDefaultAsync(p => p.ProdutoId == produtoId && p.PedidoId == pedidoId);

        public async Task<IReadOnlyCollection<Pedido>> ObterListaPorCliente(Guid clienteId)
            => await _context.Pedidos.AsNoTracking().Where(p => p.ClienteId == clienteId).ToListAsync();

        public async Task<Pedido> ObterPedidoRascunhoPorCliente(Guid clienteId)
        {
            var pedido = await _context.Pedidos.FirstOrDefaultAsync(p => p.ClienteId == clienteId && p.Status == PedidoStatus.Rascunho);
            if (pedido == null) return null;

            await _context.Entry(pedido)
                .Collection(i => i.PedidoItems).LoadAsync();

            if (pedido.VoucherId != null)
            {
                await _context.Entry(pedido)
                    .Reference(i => i.Voucher).LoadAsync();
            }

            return pedido;
        }

        public async Task<Pedido> ObterPorId(Guid id) => await _context.Pedidos.FindAsync(id);

        public async Task<Voucher> ObterVoucherPorCodigo(string codigo)
            => await _context.Vouchers.FirstOrDefaultAsync(p => p.Codigo == codigo);

        public void RemoverItem(PedidoItem pedidoItem) => _context.PedidoItems.Remove(pedidoItem);

        public void Dispose() => _context.Dispose();
    }
}