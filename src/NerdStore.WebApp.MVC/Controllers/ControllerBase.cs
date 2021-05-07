using MediatR;
using Microsoft.AspNetCore.Mvc;
using NerdStore.SharedKernel.EventHandlers;
using NerdStore.SharedKernel.Messages.Commom.Notification;
using System;

namespace NerdStore.WebApp.MVC.Controllers
{
    public abstract class ControllerBase : Controller
    {
        protected readonly DomainNotificationHandler _notifications;
        protected readonly IMediatorHandler _mediatorHandler;
        protected Guid ClienteId = Guid.Parse("A7318B61-7A0B-4100-2F44-08D784C02276");

        public ControllerBase(INotificationHandler<DomainNotification> notifications,
           IMediatorHandler mediatorHandler)
        {
            this._mediatorHandler = mediatorHandler;
            this._notifications = (DomainNotificationHandler)notifications;
        }

        protected bool OperacaoValida() => !_notifications.TemNotificacao();

        protected void NotificarErro(string codigo, string mensagem) => _mediatorHandler.Notificar(new DomainNotification(codigo, mensagem));
    }
}