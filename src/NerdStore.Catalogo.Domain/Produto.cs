using NerdStore.Catalogo.Domain.ValueObjects;
using NerdStore.SharedKernel.Assertions;
using NerdStore.SharedKernel.DomainObjects;
using NerdStore.SharedKernel.Exceptions;
using System;

namespace NerdStore.Catalogo.Domain
{
    public class Produto : Entity, IAggregateRoot
    {
        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public string Imagem { get; private set; }
        public decimal Valor { get; private set; }
        public bool Ativo { get; private set; }

        /// <summary>
        /// poderia ser controlado por um modulo que so tratasse de estoque
        /// </summary>
        public int QuantidadeEstoque { get; private set; }

        public Guid CategoriaId { get; private set; }
        public Categoria Categoria { get; private set; }

        public Dimensoes Dimensoes { get; private set; }

        protected Produto()
        {
        }

        public Produto(string nome, string descricao, string imagem, decimal valor, bool ativo, Guid categoriaId,
            Dimensoes dimensoes)
        {
            Nome = nome;
            Descricao = descricao;
            Imagem = imagem;
            Valor = valor;
            Ativo = ativo;
            CategoriaId = categoriaId;
            Dimensoes = dimensoes;

            Validar();
        }

        public void Ativar() => Ativo = true;

        public void Desativar() => Ativo = false;

        public void AlterarCategoria(Categoria categoria)
        {
            this.Categoria = categoria;
            this.CategoriaId = categoria.Id;
        }

        public void ReporEstoque(int quantidade) => QuantidadeEstoque += quantidade;

        public void AlterarDescricao(string descricao) => this.Descricao = descricao;

        public void DebitarEstoque(int quantidade)
        {
            if (quantidade < 0) quantidade *= -1;

            if (!PossuiEstoque(quantidade))
                throw new DomainException("Estoque insuficiente");

            QuantidadeEstoque -= quantidade;
        }

        public bool PossuiEstoque(int quantidade) => QuantidadeEstoque >= quantidade;

        private void Validar()
        {
            // entidade invalidas nao pode ser criadas
            // validacao
            // fail fast validaçoes - Assertion Concern -> Vernon
            // estado consistente

            ValidationBuilder.ValidarSeVazio(this.Nome, "O Nome do produto não pode estar vazio");
            ValidationBuilder.ValidarSeVazio(this.Descricao, "a Descrição do produto não pode estar vazio");
            ValidationBuilder.ValidarSeDiferente(this.CategoriaId, Guid.Empty, "Categoria não informada");
            ValidationBuilder.ValidarSeMenorQue(this.Valor, 0, "O valor está abaixo do minimo");
        }
    }
}