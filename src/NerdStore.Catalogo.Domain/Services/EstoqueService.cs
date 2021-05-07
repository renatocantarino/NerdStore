using NerdStore.Catalogo.Domain.Events;
using NerdStore.Catalogo.Domain.Repositories;
using NerdStore.SharedKernel.EventHandlers;
using System;
using System.Threading.Tasks;

namespace NerdStore.Catalogo.Domain.Services
{
    public interface IEstoqueService : IDisposable
    {
        Task<bool> Repor(Guid idProduto, int quantidade);

        Task<bool> Debitar(Guid idProduto, int quantidade);
    }

    public class EstoqueService : IEstoqueService
    {
        private const int ITEM_ESTOQUE_BAIXO_QUANTIDADE = 5;

        // expressa a necessidade de negocio
        // resolve problema que a entidade nao resolve
        // cross agreggate
        // ação conhecida da modelagem ubiqua

        private readonly IProdutoRepository _produtoRepository;
        private readonly IMediatorHandler _bus;

        public EstoqueService(IProdutoRepository produtoRepository, IMediatorHandler bus)
        {
            this._produtoRepository = produtoRepository;
            this._bus = bus;
        }

        public async Task<bool> Debitar(Guid idProduto, int quantidade)
        {
            var produto = await _produtoRepository.ObterPorId(idProduto);

            if (produto == null) return false;

            if (!produto.PossuiEstoque(quantidade)) return false;

            produto.DebitarEstoque(quantidade);
            _produtoRepository.Atualizar(produto);
            await NotificaEstoqueBaixo(produto);

            return await _produtoRepository.UnitOfWork.Commit();
        }

        private async Task NotificaEstoqueBaixo(Produto produto)
        {
            if (produto.QuantidadeEstoque < ITEM_ESTOQUE_BAIXO_QUANTIDADE)
                await _bus.Publicar(new ProdutoAbaixoEstoqueEvent(produto.Id, produto.QuantidadeEstoque));
        }

        public async Task<bool> Repor(Guid idProduto, int quantidade)
        {
            var produto = await _produtoRepository.ObterPorId(idProduto);
            if (produto == null) return false;

            produto.ReporEstoque(quantidade);
            _produtoRepository.Atualizar(produto);

            return await _produtoRepository.UnitOfWork.Commit();
        }

        public void Dispose() => _produtoRepository.Dispose();
    }
}