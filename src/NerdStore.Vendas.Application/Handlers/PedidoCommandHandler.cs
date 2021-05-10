using MediatR;
using NerdStore.SharedKernel.Commands;
using NerdStore.SharedKernel.DomainObjects.Dto;
using NerdStore.SharedKernel.EventHandlers;
using NerdStore.SharedKernel.Extensions;
using NerdStore.SharedKernel.Messages;
using NerdStore.SharedKernel.Messages.Commom.IntegrationEvents;
using NerdStore.SharedKernel.Messages.Commom.Notification;
using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Application.Events;
using NerdStore.Vendas.Domain.Entities;
using NerdStore.Vendas.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Application.Handlers
{
    public class PedidoCommandHandler : IRequestHandler<AdicionarItemPedidoCommand, ResponseBase>,
                                        IRequestHandler<AtualizarItemPedidoCommand, ResponseBase>,
                                        IRequestHandler<RemoverItemPedidoCommand, ResponseBase>,
                                        IRequestHandler<AplicarVoucherPedidoCommand, ResponseBase>,
                                        IRequestHandler<IniciarPedidoCommand, ResponseBase>
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

        public async Task<ResponseBase> Handle(AtualizarItemPedidoCommand request, CancellationToken cancellationToken)
        {
            if (!ValidateBuilder(request)) return new ResponseBase("Erro ao processar comando");

            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorCliente(request.ClienteId);

            if (pedido == null)
            {
                await _mediatorHandler.Notificar(new DomainNotification("pedido", "Não encontrado"));
                return new ResponseBase("Pedido não encontrado");
            }

            var pedidoItem = await _pedidoRepository.ObterItemPorPedido(pedido.Id, request.ProdutoId);

            if (pedidoItem != null && !pedido.ItemJaExistente(pedidoItem))
            {
                await _mediatorHandler.Notificar(new DomainNotification("pedido", "Item não encontrado"));
                return new ResponseBase("Pedido Item não encontrado");
            }

            pedido.AtualizarUnidades(pedidoItem, request.Quantidade);

            pedido.AdicionarEvento(new PedidoAtualizadoEvent(pedido.ClienteId, pedido.Id, pedido.ValorTotal));
            pedido.AdicionarEvento(new PedidoProdutoAtualizadoEvent(pedido.ClienteId, pedido.Id, request.ProdutoId,
                                                                          request.Quantidade));

            _pedidoRepository.AtualizarItem(pedidoItem);
            _pedidoRepository.Atualizar(pedido);

            await _pedidoRepository.UnitOfWork.Commit();

            return new ResponseBase(pedidoItem);
        }

        public async Task<ResponseBase> Handle(RemoverItemPedidoCommand request, CancellationToken cancellationToken)
        {
            if (!ValidateBuilder(request)) return new ResponseBase("Erro ao processar comando");

            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorCliente(request.ClienteId);

            if (pedido == null)
            {
                await _mediatorHandler.Notificar(new DomainNotification("pedido", "Não encontrado"));
                return new ResponseBase("Pedido não encontrado");
            }

            var pedidoItem = await _pedidoRepository.ObterItemPorPedido(pedido.Id, request.ProdutoId);
            if (pedidoItem != null && !pedido.ItemJaExistente(pedidoItem))
            {
                await _mediatorHandler.Notificar(new DomainNotification("pedido", "Item não encontrado"));
                return new ResponseBase("Pedido Item não encontrado");
            }

            pedido.RemoverItem(pedidoItem);

            pedido.AdicionarEvento(new PedidoAtualizadoEvent(pedido.ClienteId, pedido.Id, pedido.ValorTotal));
            pedido.AdicionarEvento(new PedidoProdutoRemovidoEvent(pedido.ClienteId, pedido.Id, request.ProdutoId));

            _pedidoRepository.RemoverItem(pedidoItem);
            _pedidoRepository.Atualizar(pedido);

            await _pedidoRepository.UnitOfWork.Commit();

            return new ResponseBase(pedido);
        }

        public async Task<ResponseBase> Handle(AplicarVoucherPedidoCommand request, CancellationToken cancellationToken)
        {
            if (!ValidateBuilder(request)) return new ResponseBase("Erro ao processar comando");

            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorCliente(request.ClienteId);
            if (pedido == null)
            {
                await _mediatorHandler.Notificar(new DomainNotification("pedido", "Não encontrado"));
                return new ResponseBase("Pedido não encontrado");
            }

            var voucher = await _pedidoRepository.ObterVoucherPorCodigo(request.Codigo);
            if (voucher == null)
            {
                await _mediatorHandler.Notificar(new DomainNotification("pedido", "Não encontrado"));
                return new ResponseBase("voucher não encontrado");
            }

            var voucherValidationResult = pedido.AplicarVoucher(voucher);
            if (!voucherValidationResult.IsValid)
            {
                foreach (var item in voucherValidationResult.Errors)
                    await _mediatorHandler.Notificar(new DomainNotification(item.ErrorCode, item.ErrorMessage));

                return new ResponseBase("Erro ao aplicar voucher");
            }

            pedido.AdicionarEvento(new PedidoAtualizadoEvent(pedido.ClienteId, pedido.Id, pedido.ValorTotal));
            pedido.AdicionarEvento(new AplicarVoucherPedidoEvent(request.ClienteId, pedido.Id, voucher.Id));

            _pedidoRepository.Atualizar(pedido);
            await _pedidoRepository.UnitOfWork.Commit();

            return new ResponseBase(pedido);
        }

        public async Task<ResponseBase> Handle(IniciarPedidoCommand request, CancellationToken cancellationToken)
        {
            if (!ValidateBuilder(request)) return new ResponseBase("Erro ao processar comando");

            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorCliente(request.ClienteId);
            pedido.Iniciar();

            var itens = new List<Item>();

            pedido.PedidoItems.ForEach(i => itens.Add(new Item { Id = i.ProdutoId, Quantidade = i.Quantidade }));
            var listaProdutoPedido = new ListaProdutoPedido { Itens = itens, Id = pedido.Id };

            pedido.AdicionarEvento(new PedidoIniciadoEvent(pedido.ClienteId, pedido.Id, request.Total, request.NomeCartao,
                                        request.NumeroCartao, request.ExpiracaoCartao, request.CvvCartao, listaProdutoPedido));

            _pedidoRepository.Atualizar(pedido);
            await _pedidoRepository.UnitOfWork.Commit();

            return new ResponseBase(request);
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