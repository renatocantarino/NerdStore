using MediatR;
using NerdStore.SharedKernel.Messages;
using System.Threading.Tasks;

namespace NerdStore.SharedKernel.EventHandlers
{
    public interface IMediatrHandler
    {
        Task Publicar<T>(T evento) where T : Event;
    }

    public class MediatrHandler : IMediatrHandler
    {
        private readonly IMediator _mediator;

        public MediatrHandler(IMediator mediator)
        {
            this._mediator = mediator;
        }

        public async Task Publicar<T>(T evento) where T : Event
        {
            await _mediator.Publish(evento);
        }
    }
}