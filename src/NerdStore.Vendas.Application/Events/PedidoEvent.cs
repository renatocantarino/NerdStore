using MediatR;
using NerdStore.SharedKernel.Messages.Commom.IntegrationEvents.Pedido;
using System.Threading;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Application.Events
{
    public class PedidoEventHandler : INotificationHandler<PedidoRascunhoIniciadoEvent>,
                                      INotificationHandler<PedidoAtualizadoEvent>,
                                      INotificationHandler<PedidoItemAdicionadoEvent>,
                                      INotificationHandler<PedidoEstoqueRejeitadoEvent>
    {
        public Task Handle(PedidoRascunhoIniciadoEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(PedidoAtualizadoEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(PedidoItemAdicionadoEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(PedidoEstoqueRejeitadoEvent notification, CancellationToken cancellationToken)
        {
            //cancela o processamento do pedido -> retorna o erro para o cliente

            return Task.CompletedTask;
        }
    }
}