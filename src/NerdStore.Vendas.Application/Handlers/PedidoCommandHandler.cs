using MediatR;
using NerdStore.SharedKernel.Commands;
using NerdStore.SharedKernel.EventHandlers;
using NerdStore.SharedKernel.Messages;
using NerdStore.SharedKernel.Messages.Commom.Notification;
using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Application.Events;
using NerdStore.Vendas.Domain.Entities;
using NerdStore.Vendas.Domain.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Application.Handlers
{
    public class PedidoCommandHandler : IRequestHandler<AdicionarItemPedidoCommand, ResponseBase>
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public PedidoCommandHandler(IPedidoRepository pedidoRepository, IMediatorHandler mediatorHandler)
        {
            this._pedidoRepository = pedidoRepository;
            this._mediatorHandler = mediatorHandler;
        }

        public async Task<ResponseBase> Handle(AdicionarItemPedidoCommand request, CancellationToken cancellationToken)
        {
            if (!ValidateBuilder(request)) return new ResponseBase("Erro ao processar comando");

            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorCliente(request.ClienteId);
            var item = new PedidoItem(request.ProdutoId, request.Nome, request.Quantidade, request.ValorUnitario);

            if (pedido == null)
            {
                pedido = Pedido.PedidoFactory.NovoPedidoRascunho(request.ClienteId);
                pedido.AdicionarItem(item);

                _pedidoRepository.Adicionar(pedido);
                pedido.AdicionarEvento(new PedidoRascunhoIniciadoEvent(request.ClienteId, pedido.Id));
            }
            else
            {
                var pedidoExistente = pedido.ItemJaExistente(item);
                pedido.AdicionarItem(item);

                if (pedidoExistente)
                    _pedidoRepository.AtualizarItem(pedido.PedidoItems.FirstOrDefault(i => i.ProdutoId == pedido.Id));
                else
                    _pedidoRepository.AdicionarItem(item);

                pedido.AdicionarEvento(new PedidoAtualizadoEvent(request.ClienteId, pedido.Id, pedido.ValorTotal));
            }

            pedido.AdicionarEvento(new PedidoItemAdicionadoEvent(pedido.ClienteId, pedido.Id,
                                                                       request.ProdutoId, request.ValorUnitario, request.Quantidade));
            await _pedidoRepository.UnitOfWork.Commit();
            return new ResponseBase(pedido);
        }

        private bool ValidateBuilder(Command msg)
        {
            if (msg.IsValid()) return true;

            foreach (var erro in msg.ValidationResult.Errors)
                _mediatorHandler.Notificar(new DomainNotification(msg.Type, erro.ErrorMessage));

            return false;
        }
    }
}