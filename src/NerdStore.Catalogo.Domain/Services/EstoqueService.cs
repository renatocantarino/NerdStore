using NerdStore.Catalogo.Domain.Events;
using NerdStore.Catalogo.Domain.Repositories;
using NerdStore.SharedKernel.DomainObjects.Dto;
using NerdStore.SharedKernel.EventHandlers;
using NerdStore.SharedKernel.Messages.Commom.Notification;
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

    public class EstoqueService : IEstoqueService
    {
        private const int ITEM_ESTOQUE_BAIXO_QUANTIDADE = 5;

        // expressa a necessidade de negocio
        // resolve problema que a entidade nao resolve
        // cross agreggate
        // ação conhecida da modelagem ubiqua

        private readonly IProdutoRepository _produtoRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public EstoqueService(IProdutoRepository produtoRepository, IMediatorHandler bus)
        {
            this._produtoRepository = produtoRepository;
            this._mediatorHandler = bus;
        }

        public async Task<bool> Debitar(Guid idProduto, int quantidade)
        {
            if (!await DebitarItemEstoque(idProduto, quantidade))
                return false;

            return await _produtoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Repor(Guid idProduto, int quantidade)
        {
            var sucesso = await ReporItemEstoque(idProduto, quantidade);

            if (!sucesso) return false;

            return await _produtoRepository.UnitOfWork.Commit();
        }

        public void Dispose() => _produtoRepository.Dispose();

        public async Task<bool> DebitarListaProdutosPedido(ListaProdutoPedido lista)
        {
            foreach (var item in lista.Itens)
            {
                if (!await Debitar(item.Id, item.Quantidade)) return false;
            }

            return await _produtoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> ReporListaProdutosPedido(ListaProdutoPedido lista)
        {
            foreach (var item in lista.Itens)
            {
                await Repor(item.Id, item.Quantidade);
            }

            return await _produtoRepository.UnitOfWork.Commit();
        }

        private async Task<bool> ReporItemEstoque(Guid produtoId, int quantidade)
        {
            var produto = await _produtoRepository.ObterPorId(produtoId);

            if (produto == null) return false;
            produto.ReporEstoque(quantidade);

            _produtoRepository.Atualizar(produto);

            return true;
        }

        private async Task<bool> DebitarItemEstoque(Guid produtoId, int quantidade)
        {
            var produto = await _produtoRepository.ObterPorId(produtoId);

            if (produto == null) return false;

            if (!produto.PossuiEstoque(quantidade))
            {
                await _mediatorHandler.Notificar(new DomainNotification("Estoque", $"Produto - {produto.Nome} sem estoque"));
                return false;
            }

            produto.DebitarEstoque(quantidade);
            await NotificaEstoqueBaixo(produto);

            _produtoRepository.Atualizar(produto);
            return true;
        }

        private async Task NotificaEstoqueBaixo(Produto produto)
        {
            if (produto.QuantidadeEstoque < ITEM_ESTOQUE_BAIXO_QUANTIDADE)
                await _mediatorHandler.Publicar(new ProdutoAbaixoEstoqueEvent(produto.Id, produto.QuantidadeEstoque));
        }
    }
}