using NerdStore.Pagamento.Business.Entities;
using NerdStore.SharedKernel.Data.Repository;

namespace NerdStore.Pagamento.Business.Repository
{
    public interface IPagamentoRepository : IRepository<Payment>
    {
        void Adicionar(Payment pagamento);

        void AdicionarTransacao(Transacao transacao);
    }
}