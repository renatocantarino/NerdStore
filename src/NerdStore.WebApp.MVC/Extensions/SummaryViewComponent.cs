using MediatR;
using Microsoft.AspNetCore.Mvc;
using NerdStore.SharedKernel.Messages.Commom.Notification;
using System.Threading.Tasks;

namespace NerdStore.WebApp.MVC.Extensions
{
    public class SummaryViewComponent : ViewComponent
    {
        private readonly DomainNotificationHandler _notification;

        public SummaryViewComponent(INotificationHandler<DomainNotification> notification) => _notification = (DomainNotificationHandler)notification;

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var notificacoes = await Task.FromResult(_notification.ObterNotificacoes());
            notificacoes.ForEach(i => ViewData.ModelState.AddModelError(string.Empty, i.Value));

            return View();
        }
    }
}