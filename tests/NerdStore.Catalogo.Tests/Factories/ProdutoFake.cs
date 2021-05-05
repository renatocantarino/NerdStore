using Bogus;
using NerdStore.Catalogo.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Catalogo.Tests.Factories
{
    public class ProdutoFake
    {
        public static Produto GetProdutoSemnome()
        {
            var fake = new Faker<Produto>("pt_BR")
                .CustomInstantiator(f => new Produto(string.Empty,
           f.Name.FullName(),
           f.Commerce.ProductAdjective(),
                f.Commerce.Random.Decimal(0.00m, 10000.00m),
     f.Random.Bool(),
     f.Random.Int(),
f.Random.Guid(),
  new Domain.ValueObjects.Dimensoes(3, 3, 6))).Generate();

            return fake;
        }

        public static Produto GetProdutoSemDescricao()
        {
            var fake = new Faker<Produto>("pt_BR")
                .CustomInstantiator(f => new Produto(f.Name.FullName(),
           string.Empty,
           f.Commerce.ProductAdjective(),
                f.Commerce.Random.Decimal(0.00m, 10000.00m),
     f.Random.Bool(), f.Random.Int(),
f.Random.Guid(),
  new Domain.ValueObjects.Dimensoes(3, 3, 6))).Generate();

            return fake;
        }
    }
}