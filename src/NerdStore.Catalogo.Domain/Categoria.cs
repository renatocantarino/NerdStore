using NerdStore.SharedKernel.Assertions;
using NerdStore.SharedKernel.DomainObjects;
using System;
using System.Collections.Generic;

namespace NerdStore.Catalogo.Domain
{
    public class Categoria : Entity
    {
        public string Nome { get; private set; }
        public int Codigo { get; private set; }

        public ICollection<Produto> Produtos { get; set; }

        protected Categoria()
        { }

        public Categoria(string nome, int codigo)
        {
            Nome = nome;
            Codigo = codigo;

            Validar();
        }

        public override string ToString() => $"{this.Nome} - {this.Codigo}";

        private void Validar()
        {
            // entidade invalidas nao pode ser criadas
            // validacao
            // fail fast validaçoes - Assertion Concern -> Vernon
            // estado consistente

            ValidationBuilder.ValidarSeVazio(this.Nome, "O Nome do produto não pode estar vazio");
            ValidationBuilder.ValidarSeIgual(Codigo, 0, "O campo Codigo não pode ser 0");
        }
    }
}