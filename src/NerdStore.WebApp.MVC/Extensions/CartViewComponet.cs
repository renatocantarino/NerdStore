using Microsoft.AspNetCore.Mvc;
using NerdStore.Vendas.Application.Queries;
using System;
using System.Threading.Tasks;

namespace NerdStore.WebApp.MVC.Extensions
{
    public class CartViewComponent : ViewComponent
    {
        private readonly IPedidoQueries _pedidoQuery;

        public CartViewComponent(IPedidoQueries pedidoQuery) => _pedidoQuery = pedidoQuery;

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cart = await _pedidoQuery.ObterCarrinhoDoCliente(Guid.NewGuid());
            var itens = cart?.Items.Count ?? 0;

            return View(itens);
        }
    }
}