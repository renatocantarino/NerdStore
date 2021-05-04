using NerdStore.SharedKernel.DomainObjects;
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

        public Produto(string nome, string descricao, string imagem, decimal valor, bool ativo, Guid categoriaId)
        {
            Nome = nome ?? throw new ArgumentNullException(nameof(nome));
            Descricao = descricao ?? throw new ArgumentNullException(nameof(descricao));
            Imagem = imagem ?? throw new ArgumentNullException(nameof(imagem));
            Valor = valor;
            Ativo = ativo;
            CategoriaId = categoriaId;
        }

        public void Ativar() => Ativo = true;

        public void Desativar() => Ativo = false;

        public void AlterarCategoria(Categoria categoria)
        {
            this.Categoria = categoria;
            this.CategoriaId = categoria.Id;
        }

        public void AlterarDescricao(string descricao) => this.Descricao = descricao;

        public void DebitarEstoque(int quantidade)
        {
            if (quantidade < 0) quantidade *= -1;
            QuantidadeEstoque -= quantidade;
        }

        public bool PossuiEstoque(int quantidade) => QuantidadeEstoque >= quantidade;

        public void Validar()
        {
            //entidade invalidas nao pode ser criadas
            //validacao
            //estado consistente
        }
    }

    public class Categoria : Entity
    {
        public string Nome { get; private set; }
        public string Descricao { get; private set; }

        public Categoria(string nome, string descricao)
        {
            Nome = nome ?? throw new ArgumentNullException(nameof(nome));
            Descricao = descricao ?? throw new ArgumentNullException(nameof(descricao));
        }

        public override string ToString() => $"{this.Nome} - {this.Descricao}";
    }
}