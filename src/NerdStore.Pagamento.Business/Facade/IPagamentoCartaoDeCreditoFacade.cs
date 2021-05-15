using NerdStore.Pagamento.Business.Dtos;
using NerdStore.Pagamento.Business.Entities;

namespace NerdStore.Pagamento.Business.Facade
{
    public interface IPagamentoCartaoDeCreditoFacade
    {
        Transacao RealizarPagamento(Pedido pedido, Payment pagamento);
    }
}