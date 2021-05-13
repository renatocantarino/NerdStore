using MediatR;
using NerdStore.Catalogo.Domain.Repositories;
using NerdStore.Catalogo.Domain.Services;
using NerdStore.SharedKernel.EventHandlers;
using NerdStore.SharedKernel.Messages.Commom.IntegrationEvents;
using NerdStore.SharedKernel.Messages.Commom.IntegrationEvents.Pedido;
using System.Threading;
using System.Threading.Tasks;

namespace NerdStore.Catalogo.Domain.Events
{
    public class ProdutoEventHandler : INotificationHandler<ProdutoAbaixoEstoqueEvent>,
                                       INotificationHandler<PedidoIniciadoEvent>
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IEstoqueService _estoqueService;
        private readonly IMediatorHandler _mediatorHandler;

        public ProdutoEventHandler(IProdutoRepository produtoRepository, IEstoqueService estoqueService,
            IMediatorHandler mediatorHandler)
        {
            this._produtoRepository = produtoRepository;
            this._estoqueService = estoqueService;
            this._mediatorHandler = mediatorHandler;
        }

        public async Task Handle(ProdutoAbaixoEstoqueEvent notification, CancellationToken cancellationToken)
        {
            //manda email, sms, faz tudo oq quiser
            var produto = await _produtoRepository.ObterPorId(notification.AggregateId);
            //aquisição de mais produto deste tipo
        }

        public async Task Handle(PedidoIniciadoEvent notification, CancellationToken cancellationToken)
        {
            var debitarEstoque = await _estoqueService.DebitarListaProdutosPedido(notification.ListaProdutoPedido);

            if (debitarEstoque)
                await _mediatorHandler.Publicar(new PedidoEstoqueConfirmadoEvent(notification.ClienteId, notification.PedidoId,
                                                                                notification.Total, notification.TitularCartao,
                                                                                notification.NumeroCartao, notification.ExpiracaoCartao,
                                                                                notification.CvvCartao, notification.ListaProdutoPedido));
            else
                await _mediatorHandler.Publicar(new PedidoEstoqueRejeitadoEvent(notification.PedidoId, notification.ClienteId));
        }
    }
}