using NerdStore.Catalogo.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NerdStore.Catalogo.Application.Services
{
    public interface IProdutoAppService : IDisposable
    {
        Task<IReadOnlyCollection<ProdutoViewModel>> ObterPorCategoria(int codigo);

        Task<ProdutoViewModel> ObterPorId(Guid id);

        Task<IReadOnlyCollection<ProdutoViewModel>> ObterTodos();

        Task<IReadOnlyCollection<CategoriaViewModel>> ObterCategorias();

        Task AdicionarProduto(ProdutoViewModel produtoViewModel);

        Task AtualizarProduto(ProdutoViewModel produtoViewModel);

        Task<ProdutoViewModel> DebitarEstoque(Guid id, int quantidade);

        Task<ProdutoViewModel> ReporEstoque(Guid id, int quantidade);
    }
}