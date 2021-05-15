using NerdStore.Pagamento.Business.Entities;
using NerdStore.SharedKernel.DomainObjects.Dto;
using System;
using System.Threading.Tasks;

namespace NerdStore.Pagamento.Business.Service
{
    public interface IPagamentoService
    {
        Task<Transacao> RealizarPagamentoPedido(PagamentoPedido pagamentoPedido);
    }
}