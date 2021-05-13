using NerdStore.SharedKernel.DomainObjects.Dto;
using System;
using System.Threading.Tasks;

namespace NerdStore.Catalogo.Domain.Services
{
    public interface IEstoqueService : IDisposable
    {
        Task<bool> Repor(Guid idProduto, int quantidade);

        Task<bool> Debitar(Guid idProduto, int quantidade);

        Task<bool> DebitarListaProdutosPedido(ListaProdutoPedido lista);

        Task<bool> ReporListaProdutosPedido(ListaProdutoPedido lista);
    }
}