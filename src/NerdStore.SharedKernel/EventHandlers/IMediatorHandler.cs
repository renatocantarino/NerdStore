using NerdStore.SharedKernel.Commands;
using NerdStore.SharedKernel.Messages;
using NerdStore.SharedKernel.Messages.Commom.Notification;
using System.Threading.Tasks;

namespace NerdStore.SharedKernel.EventHandlers
{
    public interface IMediatorHandler
    {
        Task Publicar<T>(T evento) where T : Event;

        Task Comando<T>(T evento) where T : Command;

        Task Notificar<T>(T evento) where T : DomainNotification;
    }
}