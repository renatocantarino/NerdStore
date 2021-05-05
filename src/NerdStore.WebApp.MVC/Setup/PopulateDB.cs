using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NerdStore.Catalogo.Data.Contexts;
using NerdStore.Catalogo.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

            Produto regata = new("Regata BS", "regata branca", "", 55.6m, true, blusa.Id, new(1, 3, 1));
            Produto camisa = new("camisa BS", "camisa branca", "", 75.6m, true, blusa.Id, new(1, 3, 1));
            Produto caneca = new("caneca BS", "caneca branca", "", 25.6m, true, acessorios.Id, new(1, 3, 1));
            Produto moleton = new("moleton BS", "moleton branca", "", 95.6m, true, casacos.Id, new(1, 3, 1));
            Produto bermuda = new("bermuda BS", "bermuda branca", "", 45.6m, true, blusa.Id, new(1, 3, 1));

            context.Produtos.Add(regata);
            context.Produtos.Add(camisa);
            context.Produtos.Add(caneca);
            context.Produtos.Add(moleton);
            context.Produtos.Add(bermuda);

            context.SaveChanges();
        }
    }
}