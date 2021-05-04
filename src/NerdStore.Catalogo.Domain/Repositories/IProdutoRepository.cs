using NerdStore.SharedKernel.Data.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NerdStore.Catalogo.Domain.Repositories
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<IReadOnlyCollection<Produto>> ObterTodos();

        Task<Produto> ObterPorId(Guid id);

        Task<IReadOnlyCollection<Produto>> ObterPorCategoria(int codigo);

        Task<IReadOnlyCollection<Categoria>> ObterCategorias();

        void Adicionar(Produto produto);

        void Atualizar(Produto produto);

        void Adicionar(Categoria categoria);

        void Atualizar(Categoria categoria);
    }
}