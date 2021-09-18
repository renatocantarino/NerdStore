using MediatR;
using NerdStore.Pagamento.Business.Service;
using NerdStore.SharedKernel.DomainObjects.Dto;
using NerdStore.SharedKernel.Messages.Commom.IntegrationEvents.Pedido;
using System.Threading;
using System.Threading.Tasks;

namespace NerdStore.Pagamento.Business.Events
{
    public class PagamentoEventHandler : INotificationHandler<PedidoEstoqueConfirmadoEvent>
    {
        private readonly IPagamentoService _pagamentoService;

        public PagamentoEventHandler(IPagamentoService pagamentoService)
        {
            _pagamentoService = pagamentoService;
        }

        public async Task Handle(PedidoEstoqueConfirmadoEvent message, CancellationToken cancellationToken)
        {
            var pagamentoPedido = new PagamentoPedido()
            {
                PedidoId = message.PedidoId,
                ClienteId = message.ClienteId,
                Total = message.Total,
                CvvCartao = message.CvvCartao,
                ExpiracaoCartao = message.ExpiracaoCartao,
                NomeCartao = message.TitularCartao,
                NumeroCartao = message.NumeroCartao
            };

            await _pagamentoService.RealizarPagamentoPedido(pagamentoPedido);
        }
    }
}