using NerdStore.Vendas.Application.Queries.ViewModels;
using NerdStore.Vendas.Domain.Repositories;
using NerdStore.Vendas.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Application.Queries
{
    public class PedidoQueries : IPedidoQueries
    {
        private readonly IPedidoRepository _pedidoRepository;

        public PedidoQueries(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        public async Task<CarrinhoViewModel> ObterCarrinhoDoCliente(Guid clienteId)
        {
            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorCliente(clienteId);
            if (pedido == null) return null;

            var carrinho = new CarrinhoViewModel
            {
                ClienteId = pedido.ClienteId,
                ValorTotal = pedido.ValorTotal,
                ValorDesconto = pedido.Desconto,
                PedidoId = pedido.Id,
                SubTotal = pedido.Desconto + pedido.ValorTotal,
                Voucher = pedido.VoucherId.HasValue ? pedido.VoucherId.Value.ToString() : string.Empty
            };

            carrinho.Items.AddRange(from item in pedido.PedidoItems
                                    select new CarrinhoItemViewModel
                                    {
                                        ProdutoId = item.ProdutoId,
                                        ProdutoNome = item.Nome,
                                        Quantidade = item.Quantidade,
                                        ValorUnitario = item.ValorUnitario,
                                        ValorTotal = item.ValorUnitario + item.Quantidade
                                    });
            return carrinho;
        }

        public async Task<IEnumerable<PedidoViewModel>> ObterPedidosDoCliente(Guid clienteId)
        {
            var pedidos = await _pedidoRepository.ObterListaPorCliente(clienteId);
            if (pedidos == null) return null;

            pedidos = pedidos.Where(x => x.Status == PedidoStatus.Pago || x.Status == PedidoStatus.Cancelado)
                             .OrderByDescending(p => p.Codigo);

            if (!pedidos.Any()) return null;

            var pedidosVm = (from item in pedidos
                             select new PedidoViewModel
                             {
                                 ValorTotal = item.ValorTotal,
                                 Status = (int)item.Status,
                                 Codigo = item.Codigo,
                                 DataCadastro = item.DataCadastro
                             }).ToList();

            return pedidosVm;
        }
    }
}