using NerdStore.Pagamento.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Pagamento.Business.Dtos
{
    public class DadosOperacao
    {
        public DadosOperacao(Pedido pedido, Payment pagamento, Transacao transacao)
        {
            Pedido = pedido;
            Pagamento = pagamento;
            Transacao = transacao;
        }

        public Pedido Pedido { get; }
        public Payment Pagamento { get; }
        public Transacao Transacao { get; }
    }
}