using AutoMapper;
using NerdStore.Catalogo.Application.ViewModels;
using NerdStore.Catalogo.Domain;
using NerdStore.Catalogo.Domain.Repositories;
using NerdStore.Catalogo.Domain.Services;
using NerdStore.SharedKernel.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NerdStore.Catalogo.Application.Services
{
    public class ProdutoAppService : IProdutoAppService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMapper _mapper;
        private readonly IEstoqueService _estoqueService;

        public ProdutoAppService(IProdutoRepository produtoRepository, IMapper mapper,
            IEstoqueService estoqueService)
        {
            this._produtoRepository = produtoRepository;
            this._mapper = mapper;
            this._estoqueService = estoqueService;
        }

        public async Task AdicionarProduto(ProdutoViewModel produtoViewModel)
        {
            var produto = _mapper.Map<Produto>(produtoViewModel);
            _produtoRepository.Adicionar(produto);

            await _produtoRepository.UnitOfWork.Commit();
        }

        public async Task AtualizarProduto(ProdutoViewModel produtoViewModel)
        {
            var produto = _mapper.Map<Produto>(produtoViewModel);
            _produtoRepository.Atualizar(produto);

            await _produtoRepository.UnitOfWork.Commit();
        }

        public async Task<ProdutoViewModel> DebitarEstoque(Guid id, int quantidade)
        {
            var operacaoDebitar = await _estoqueService.Debitar(id, quantidade);
            if (!operacaoDebitar)
                throw new DomainException("Falha ao debitar estoque");

            return _mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterPorId(id));
        }

        public async Task<IReadOnlyCollection<CategoriaViewModel>> ObterCategorias()
        {
            return _mapper.Map<IReadOnlyCollection<CategoriaViewModel>>(await _produtoRepository.ObterCategorias());
        }

        public async Task<IReadOnlyCollection<ProdutoViewModel>> ObterPorCategoria(int codigo)
        {
            return _mapper.Map<IReadOnlyCollection<ProdutoViewModel>>(await _produtoRepository.ObterPorCategoria(codigo));
        }

        public async Task<ProdutoViewModel> ObterPorId(Guid id)
        {
            return _mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterPorId(id));
        }

        public async Task<IReadOnlyCollection<ProdutoViewModel>> ObterTodos()
        {
            return _mapper.Map<IReadOnlyCollection<ProdutoViewModel>>(await _produtoRepository.ObterTodos());
        }

        public async Task<ProdutoViewModel> ReporEstoque(Guid id, int quantidade)
        {
            var operacaoReposicao = await _estoqueService.Debitar(id, quantidade);
            if (!operacaoReposicao)
                throw new DomainException("Falha ao repor estoque");

            return _mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterPorId(id));
        }

        public void Dispose()
        {
            _produtoRepository?.Dispose();
            _estoqueService?.Dispose();
        }
    }
}