using NerdStore.Catalogo.Tests.Factories;
using NerdStore.SharedKernel.Exceptions;
using Xunit;

namespace NerdStore.Catalogo.Tests
{
    public class ProdutoUnitTest
    {
        [Fact]
        public void Produto_Invalido_DeveRetornarExcecões()
        {
            var semNome = Assert.Throws<DomainException>(() => ProdutoFake.GetProdutoSemnome());

            string erroProdutoSemNome = "O Nome do produto não pode estar vazio";

            Assert.Equal(erroProdutoSemNome, semNome.Message);

            var semDescricao = Assert.Throws<DomainException>(() => ProdutoFake.GetProdutoSemDescricao());

            string erroProdutosemDescricao = "a Descrição do produto não pode estar vazio";

            Assert.Equal(erroProdutosemDescricao, semDescricao.Message);
        }
    }
}