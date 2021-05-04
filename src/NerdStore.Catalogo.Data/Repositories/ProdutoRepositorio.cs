using Microsoft.EntityFrameworkCore;
using NerdStore.Catalogo.Data.Contexts;
using NerdStore.Catalogo.Domain;
using NerdStore.Catalogo.Domain.Repositories;
using NerdStore.SharedKernel.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NerdStore.Catalogo.Data.Repositories
{
    public class ProdutoRepositorio : IProdutoRepository
    {
        private readonly CatalogoContext _context;

        public ProdutoRepositorio(CatalogoContext context)
        {
            this._context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Adicionar(Produto produto) => _context.Produtos.Add(produto);

        public void Atualizar(Produto produto) => _context.Produtos.Update(produto);

        public void Adicionar(Categoria categoria) => _context.Categorias.Add(categoria);

        public void Atualizar(Categoria categoria) => _context.Categorias.Update(categoria);

        public async Task<IReadOnlyCollection<Categoria>> ObterCategorias() => await _context.Categorias.AsNoTracking().ToListAsync();

        public async Task<IReadOnlyCollection<Produto>> ObterPorCategoria(int codigo)
        {
            return await _context.Produtos.AsNoTracking()
                .Include(c => c.Categoria)
                .Where(p => p.Categoria.Codigo == codigo)
                .ToListAsync();
        }

        public async Task<Produto> ObterPorId(Guid id) => await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        public async Task<IReadOnlyCollection<Produto>> ObterTodos() => await _context.Produtos.AsNoTracking().ToListAsync();

        public void Dispose() => _context?.Dispose();
    }
}