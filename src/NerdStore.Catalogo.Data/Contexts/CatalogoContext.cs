using Microsoft.EntityFrameworkCore;
using NerdStore.Catalogo.Domain;
using NerdStore.SharedKernel.Data.Repository;
using System.Linq;
using System.Threading.Tasks;

namespace NerdStore.Catalogo.Data.Contexts
{
    public class CatalogoContext : DbContext, IUnitOfWork
    {
        public CatalogoContext(DbContextOptions<CatalogoContext> opts) : base(opts)
        { }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
            {
                property.SetColumnType("varchar(100)");
            }

            base.OnModelCreating(modelBuilder);
        }

        public Task Commit()
        {
            throw new System.NotImplementedException();
        }
    }
}