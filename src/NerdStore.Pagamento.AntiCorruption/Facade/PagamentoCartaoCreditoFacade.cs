using NerdStore.Pagamento.AntiCorruption.Configurations;
using NerdStore.Pagamento.AntiCorruption.Gateway.Paypal;
using NerdStore.Pagamento.Business.Dtos;
using NerdStore.Pagamento.Business.Entities;
using NerdStore.Pagamento.Business.Facade;

namespace NerdStore.Pagamento.AntiCorruption.Facade
{
    public class PagamentoCartaoCreditoFacade : IPagamentoCartaoDeCreditoFacade
    {
        private readonly IPayPalGateway _payPalGateway;
        private readonly IConfigurationManager _configurationManager;

        public PagamentoCartaoCreditoFacade(IPayPalGateway payPalGateway, IConfigurationManager configurationManager)
        {
            _payPalGateway = payPalGateway;
            _configurationManager = configurationManager;
        }

        public Transacao RealizarPagamento(Pedido pedido, Payment pagamento)
        {
            var apikey = _configurationManager.GetValue("apiKey");
            var encriptedKey = _configurationManager.GetValue("encriptedKey");

            var serviceKey = _payPalGateway.GetPayPalServiceKey(apikey, encriptedKey);
            var cardHash = _payPalGateway.GetCardHashKey(serviceKey, pagamento.NumeroCartao);

            var pagamentoResult = _payPalGateway.CommitTransaction(cardHash, pedido.Id.ToString(), pedido.Valor);

            //o gateway retorna um objeto com dados do pagamento : se foi recusado, aprovado e dados cartao

            var transacao = new Transacao
            {
                PedidoId = pedido.Id,
                Total = pedido.Valor,
                PagamentoId = pagamento.Id
            };

            transacao.StatusTransacao = pagamentoResult ? StatusTransacao.Pago
                                                        : StatusTransacao.Recusado;

            return transacao;
        }
    }
}