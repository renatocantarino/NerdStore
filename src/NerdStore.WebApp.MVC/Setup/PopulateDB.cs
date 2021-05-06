using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NerdStore.Catalogo.Data.Contexts;
using NerdStore.Catalogo.Domain;
using System.Linq;

namespace NerdStore.WebApp.MVC.Setup
{
    public static class PopulateDB
    {
        public static void Seed(this IApplicationBuilder app, bool development = false)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<CatalogoContext>();
            context.Seed();
        }

        private static void Seed(this CatalogoContext context)
        {
            if (context.Categorias.Any()) return;

            Categoria blusa = new("Blusas", 1);
            Categoria acessorios = new("acessorios", 2);
            Categoria casacos = new("Casacos", 3);

            context.Categorias.Add(blusa);
            context.Categorias.Add(acessorios);
            context.Categorias.Add(casacos);

            context.SaveChanges();

            Produto regata = new("Regata BS", "regata branca", "camiseta1.jpg", 55.6m, true, 1, blusa.Id, new(1, 3, 1)); ;
            Produto camisa = new("camisa BS", "camisa branca", "camiseta2.jpg", 75.6m, true, 5, blusa.Id, new(1, 3, 1));
            Produto caneca = new("caneca BS", "caneca branca", "caneca3.jpg", 25.6m, true, 7, acessorios.Id, new(1, 3, 1));
            Produto moleton = new("moleton BS", "moleton branca", "camiseta3.jpg", 95.6m, true, 3, casacos.Id, new(1, 3, 1));
            Produto bermuda = new("bermuda BS", "bermuda branca", "camiseta4.jpg", 45.6m, true, 2, blusa.Id, new(1, 3, 1));

            context.Produtos.Add(regata);
            context.Produtos.Add(camisa);
            context.Produtos.Add(caneca);
            context.Produtos.Add(moleton);
            context.Produtos.Add(bermuda);

            context.SaveChanges();
        }
    }
}