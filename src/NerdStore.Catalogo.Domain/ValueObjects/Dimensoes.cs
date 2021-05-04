using NerdStore.SharedKernel.Assertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Catalogo.Domain.ValueObjects
{
    public class Dimensoes
    {
        public decimal Altura { get; private set; }
        public decimal Largura { get; private set; }
        public decimal Profundidade { get; private set; }

        public Dimensoes(decimal altura, decimal largura, decimal profundidade)
        {
            ValidationBuilder.ValidarSeMenorQue(this.Altura, 0, "Altura não permitida");
            ValidationBuilder.ValidarSeMenorQue(this.Largura, 0, "Largura não permitida");
            ValidationBuilder.ValidarSeMenorQue(this.Profundidade, 0, "Profundidade não permitida");

            Altura = altura;
            Largura = largura;
            Profundidade = profundidade;
        }

        public string Formatado => $"LXAXP: {this.Largura} x  {this.Altura} x {this.Profundidade}";

        public override string ToString()
        {
            return Formatado;
        }
    }
}