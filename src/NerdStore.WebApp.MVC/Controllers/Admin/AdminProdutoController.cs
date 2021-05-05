using Microsoft.AspNetCore.Mvc;
using NerdStore.Catalogo.Application.Services;
using NerdStore.Catalogo.Application.ViewModels;
using System;
using System.Threading.Tasks;

namespace NerdStore.WebApp.MVC.Controllers.Admin
{
    public class AdminProdutoController : Controller
    {
        private readonly IProdutoAppService _produtoAppService;

        public AdminProdutoController(IProdutoAppService produtoAppService) => this._produtoAppService = produtoAppService;

        [HttpGet]
        [Route("admin-produtos")]
        public async Task<IActionResult> Index()
        {
            return View(await _produtoAppService.ObterTodos());
        }

        [HttpGet]
        [Route("novo-produto")]
        public async Task<IActionResult> Novo()
        {
            return View(await BindCategoria(new ProdutoViewModel()));
        }

        [HttpPost]
        [Route("novo-produto")]
        public async Task<IActionResult> Novo(ProdutoViewModel produtoViewModel)
        {
            if (!ModelState.IsValid)
                return View(await BindCategoria(produtoViewModel));

            await _produtoAppService.AdicionarProduto(produtoViewModel);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("produto-atualiza-estoque")]
        public async Task<IActionResult> Estoque(Guid idProduto, int quantidade)
        {
            if (quantidade > 0)
                await _produtoAppService.ReporEstoque(idProduto, quantidade);
            else
                await _produtoAppService.DebitarEstoque(idProduto, quantidade);

            return View("Index", _produtoAppService.ObterTodos());
        }

        private async Task<ProdutoViewModel> BindCategoria(ProdutoViewModel produtoViewModel)
        {
            produtoViewModel.Categorias = await _produtoAppService.ObterCategorias();

            return produtoViewModel;
        }
    }
}