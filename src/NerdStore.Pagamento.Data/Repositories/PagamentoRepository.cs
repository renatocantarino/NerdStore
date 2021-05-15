using NerdStore.Pagamento.Business.Entities;
using NerdStore.Pagamento.Business.Repository;
using NerdStore.Pagamento.Data.Contexts;
using NerdStore.SharedKernel.Data.Repository;
using System;

namespace NerdStore.Pagamento.Data.Repositories
{
    public class PagamentoRepository : IPagamentoRepository
    {
        private readonly PagamentoContext _pagamentoContext;

        public PagamentoRepository(PagamentoContext pagamentoContext) => _pagamentoContext = pagamentoContext;

        public IUnitOfWork UnitOfWork => _pagamentoContext;

        public void Adicionar(Payment pagamento) => _pagamentoContext.Pagamentos.Add(pagamento);

        public void AdicionarTransacao(Transacao transacao) => _pagamentoContext.Transacoes.Add(transacao);

        public void Dispose() => _pagamentoContext?.Dispose();
    }
}