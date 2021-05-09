using NerdStore.Vendas.Application.Queries.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Application.Queries
{
    public interface IPedidoQueries
    {
        Task<CarrinhoViewModel> ObterCarrinhoDoCliente(Guid clienteId);

        Task<IEnumerable<PedidoViewModel>> ObterPedidosDoCliente(Guid clienteId);
    }
}