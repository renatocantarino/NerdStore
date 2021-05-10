using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NerdStore.Catalogo.Application.Services;
using NerdStore.Catalogo.Data.Contexts;
using NerdStore.Catalogo.Data.Repositories;
using NerdStore.Catalogo.Domain.Events;
using NerdStore.Catalogo.Domain.Repositories;
using NerdStore.Catalogo.Domain.Services;
using NerdStore.SharedKernel.EventHandlers;
using NerdStore.SharedKernel.Messages;
using NerdStore.SharedKernel.Messages.Commom.Notification;
using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Application.Handlers;
using NerdStore.Vendas.Application.Queries;
using NerdStore.Vendas.Data.Contexts;
using NerdStore.Vendas.Data.Repository;
using NerdStore.Vendas.Domain.Repositories;

namespace NerdStore.WebApp.MVC.Setup
{
    public static class IoC
    {
        public static void RegisterServices(this IServiceCollection service)
        {
            service.AddScoped<IMediatorHandler, MediatorHandler>();

            //domain notifications
            service.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            //catalogo
            service.AddScoped<IProdutoRepository, ProdutoRepositorio>();
            service.AddScoped<IProdutoAppService, ProdutoAppService>();
            service.AddScoped<IEstoqueService, EstoqueService>();
            service.AddScoped<CatalogoContext>();

            service.AddScoped<INotificationHandler<ProdutoAbaixoEstoqueEvent>, ProdutoEventHandler>();

            //vendas
            service.AddScoped<IPedidoRepository, PedidoRepository>();
            service.AddScoped<IPedidoQueries, PedidoQueries>();
            service.AddScoped<VendasContext>();

            service.AddScoped<IRequestHandler<AdicionarItemPedidoCommand, ResponseBase>, PedidoCommandHandler>();
            service.AddScoped<IRequestHandler<AtualizarItemPedidoCommand, ResponseBase>, PedidoCommandHandler>();
            service.AddScoped<IRequestHandler<RemoverItemPedidoCommand, ResponseBase>, PedidoCommandHandler>();
            service.AddScoped<IRequestHandler<AplicarVoucherPedidoCommand, ResponseBase>, PedidoCommandHandler>();
        }
    }
}