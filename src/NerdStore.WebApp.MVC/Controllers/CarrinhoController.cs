using MediatR;
using Microsoft.AspNetCore.Mvc;
using NerdStore.Catalogo.Application.Services;
using NerdStore.Catalogo.Application.ViewModels;
using NerdStore.SharedKernel.EventHandlers;
using NerdStore.SharedKernel.Messages.Commom.Notification;
using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Application.Queries;
using System;
using System.Threading.Tasks;

namespace NerdStore.WebApp.MVC.Controllers
{
    public class CarrinhoController : ControllerBase
    {
        private readonly IProdutoAppService _produtoAppService;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IPedidoQueries _pedidoQueries;

        public CarrinhoController(INotificationHandler<DomainNotification> notifications,
                                  IProdutoAppService produtoAppService, IMediatorHandler mediatorHandler,
                                  IPedidoQueries pedidoQueries) : base(notifications, mediatorHandler)

        {
            _produtoAppService = produtoAppService;
            _mediatorHandler = mediatorHandler;
            this._pedidoQueries = pedidoQueries;
        }

        [Route("meu-carrinho")]
        public async Task<IActionResult> Index() => View(await _pedidoQueries.ObterCarrinhoDoCliente(ClienteId));

        [HttpPost]
        [Route("meu-carrinho")]
        public async Task<IActionResult> AdicionarItem(Guid id, int quantidade)
        {
            var produto = await _produtoAppService.ObterPorId(id);
            if (produto == null) return BadRequest();

            if (produto.QuantidadeEstoque < quantidade)
            {
                TempData["Erro"] = "Produto com estoque insuficiente para atender sua compra";
                return RedirectToAction("Detalhes", "Vitrine", new { id });
            }

            await _mediatorHandler.Comando(GetCommand(produto, quantidade));

            if (OperacaoValida())
                return RedirectToAction("Index");

            TempData["Erro"] = "Produto indisponivel";
            return RedirectToAction("Detalhes", "Vitrine", new { id });
        }

        [HttpPost]
        [Route("remover-item")]
        public async Task<IActionResult> RemoverItem(Guid id)
        {
            var produto = await _produtoAppService.ObterPorId(id);
            if (produto == null) return BadRequest();

            var command = new RemoverItemCommand();
        }

        private AdicionarItemPedidoCommand GetCommand(ProdutoViewModel produto, int quantidade)
               => new(ClienteId, produto.Id, produto.Nome, quantidade, produto.Valor);
    }
}