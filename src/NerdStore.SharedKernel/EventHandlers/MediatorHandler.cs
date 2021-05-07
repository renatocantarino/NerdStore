using MediatR;
using NerdStore.SharedKernel.Commands;
using NerdStore.SharedKernel.Messages;
using NerdStore.SharedKernel.Messages.Commom.Notification;
using System.Threading.Tasks;

namespace NerdStore.SharedKernel.EventHandlers
{
    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;

        public MediatorHandler(IMediator mediator)
        {
            this._mediator = mediator;
        }

        public async Task Comando<T>(T command) where T : Command
        {
            await _mediator.Send(command);
        }

        public async Task Notificar<T>(T evento) where T : DomainNotification
        {
            await _mediator.Publish(evento);
        }

        public async Task Publicar<T>(T evento) where T : Event
        {
            await _mediator.Publish(evento);
        }
    }
}