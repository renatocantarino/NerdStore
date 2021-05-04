using NerdStore.Catalogo.Domain;
using NerdStore.Catalogo.Domain.ValueObjects;
using NerdStore.Catalogo.Tests.Factories;
using NerdStore.SharedKernel.Exceptions;
using System;
using Xunit;

namespace NerdStore.Catalogo.Tests
{
    public class ProdutoUnitTest
    {
        [Fact]
        public void Produto_Invalido_DeveRetornarExcec�es()
        {
            var semNome = Assert.Throws<DomainException>(
                () => ProdutoFake.GetProdutoSemnome());

            string erroProdutoSemNome = "O Nome do produto n�o pode estar vazio";

            Assert.Equal(erroProdutoSemNome, semNome.Message);
        }
    }
}