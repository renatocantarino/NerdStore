using MediatR;
using Microsoft.AspNetCore.Mvc;
using NerdStore.Catalogo.Application.Services;
using NerdStore.Catalogo.Application.ViewModels;
using NerdStore.SharedKernel.EventHandlers;
using NerdStore.SharedKernel.Messages.Commom.Notification;
using NerdStore.Vendas.Application.Commands;
using System;
using System.Threading.Tasks;

namespace NerdStore.WebApp.MVC.Controllers
{
    public class CarrinhoController : ControllerBase
    {
        private readonly IProdutoAppService _produtoAppService;
        private readonly IMediatorHandler _mediatorHandler;

        public CarrinhoController(INotificationHandler<DomainNotification> notifications,
               IProdutoAppService produtoAppService, IMediatorHandler mediatorHandler) : base(notifications, mediatorHandler)

        {
            _produtoAppService = produtoAppService;
            _mediatorHandler = mediatorHandler;
        }

        public IActionResult Index() => View();

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

        private AdicionarItemPedidoCommand GetCommand(ProdutoViewModel produto, int quantidade)
        {
            return new AdicionarItemPedidoCommand(ClienteId, produto.Id, produto.Nome,
                                                  quantidade, produto.Valor);
        }
    }
}