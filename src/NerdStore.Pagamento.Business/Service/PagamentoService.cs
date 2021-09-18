using NerdStore.Pagamento.Business.Dtos;
using NerdStore.Pagamento.Business.Entities;
using NerdStore.Pagamento.Business.Facade;
using NerdStore.Pagamento.Business.Repository;
using NerdStore.SharedKernel.DomainObjects.Dto;
using NerdStore.SharedKernel.EventHandlers;
using NerdStore.SharedKernel.Messages.Commom.IntegrationEvents.Pagamento;
using NerdStore.SharedKernel.Messages.Commom.Notification;
using System.Threading.Tasks;

namespace NerdStore.Pagamento.Business.Service
{
    public class PagamentoService : IPagamentoService
    {
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IPagamentoCartaoDeCreditoFacade _pagamentoFacade;
        private readonly IPagamentoRepository _pagamentoRepository;

        public PagamentoService(IMediatorHandler mediatorHandler, IPagamentoCartaoDeCreditoFacade pagamentoFacade, IPagamentoRepository pagamentoRepository)
        {
            _mediatorHandler = mediatorHandler;
            _pagamentoFacade = pagamentoFacade;
            _pagamentoRepository = pagamentoRepository;
        }

        public async Task<Transacao> RealizarPagamentoPedido(PagamentoPedido pagamentoPedido)
        {
            var pedido = new Pedido
            {
                Id = pagamentoPedido.PedidoId,
                Valor = pagamentoPedido.Total,
                ClienteId = pagamentoPedido.ClienteId
            };

            var pagamento = new Payment
            {
                Valor = pagamentoPedido.Total,
                NomeCartao = pagamentoPedido.NomeCartao,
                NumeroCartao = pagamentoPedido.NumeroCartao,
                ExpiracaoCartao = pagamentoPedido.ExpiracaoCartao,
                CvvCartao = pagamentoPedido.CvvCartao,
                PedidoId = pagamentoPedido.PedidoId
            };

            var transacao = _pagamentoFacade.RealizarPagamento(pedido, pagamento);

            var _dadosOperacao = new DadosOperacao(pedido, pagamento, transacao);

            await TransacaoComSucesso(_dadosOperacao);
            TransacaoRecusada(_dadosOperacao);

            return _dadosOperacao.Transacao;
        }

        private async void TransacaoRecusada(DadosOperacao dadosOperacao)
        {
            if (dadosOperacao.Transacao.StatusTransacao != StatusTransacao.Recusado) return;

            await _mediatorHandler.Notificar(new DomainNotification("pagamento", "operadora recusou o pagamento"));
            await _mediatorHandler.Publicar(new PagamentoRecusadoEvent(dadosOperacao.Pedido.Id,
                                                                              dadosOperacao.Pedido.ClienteId,
                                                                              dadosOperacao.Pagamento.Id,
                                                                              dadosOperacao.Transacao.Id, dadosOperacao.Pedido.Valor));
        }

        private async Task<bool> TransacaoComSucesso(DadosOperacao dadosOperacao)
        {
            if (dadosOperacao.Transacao.StatusTransacao != StatusTransacao.Pago) return false;

            dadosOperacao.Pagamento.AdicionarEvento(new PagamentoRealizadoEvent(dadosOperacao.Pedido.Id, dadosOperacao.Pedido.ClienteId,
                                                                                      dadosOperacao.Pagamento.Id, dadosOperacao.Transacao.Id,
                                                                                      dadosOperacao.Pedido.Valor));

            _pagamentoRepository.Adicionar(dadosOperacao.Pagamento);
            _pagamentoRepository.AdicionarTransacao(dadosOperacao.Transacao);

            return await _pagamentoRepository.UnitOfWork.Commit();
        }
    }
}