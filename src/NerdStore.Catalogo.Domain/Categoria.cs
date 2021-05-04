using NerdStore.SharedKernel.Assertions;
using NerdStore.SharedKernel.DomainObjects;
using System;

namespace NerdStore.Catalogo.Domain
{
    public class Categoria : Entity
    {
        public string Nome { get; private set; }
        public string Descricao { get; private set; }

        public Categoria(string nome, string descricao)
        {
            Nome = nome;
            Descricao = descricao;

            Validar();
        }

        public override string ToString() => $"{this.Nome} - {this.Descricao}";

        private void Validar()
        {
            // entidade invalidas nao pode ser criadas
            // validacao
            // fail fast validaçoes - Assertion Concern -> Vernon
            // estado consistente

            ValidationBuilder.ValidarSeVazio(this.Nome, "O Nome do produto não pode estar vazio");
            ValidationBuilder.ValidarSeVazio(this.Descricao, "a Descrição do produto não pode estar vazio");
        }
    }
}