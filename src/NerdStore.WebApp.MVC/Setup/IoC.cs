using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NerdStore.Catalogo.Application.Services;
using NerdStore.Catalogo.Data.Contexts;
using NerdStore.Catalogo.Data.Repositories;
using NerdStore.Catalogo.Domain.Events;
using NerdStore.Catalogo.Domain.Repositories;
using NerdStore.Catalogo.Domain.Services;
using NerdStore.SharedKernel.EventHandlers;

namespace NerdStore.WebApp.MVC.Setup
{
    public static class IoC
    {
        public static void RegisterServices(this IServiceCollection service)
        {
            service.AddScoped<IMediatrHandler, MediatrHandler>();

            //catalogo
            service.AddScoped<IProdutoRepository, ProdutoRepositorio>();
            service.AddScoped<IProdutoAppService, ProdutoAppService>();
            service.AddScoped<IEstoqueService, EstoqueService>();
            service.AddScoped<CatalogoContext>();

            service.AddScoped<INotificationHandler<ProdutoAbaixoEstoqueEvent>, ProdutoEventHandler>();
        }
    }
}