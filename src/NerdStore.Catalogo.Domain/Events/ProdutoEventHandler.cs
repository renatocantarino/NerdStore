using MediatR;
using NerdStore.Catalogo.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace NerdStore.Catalogo.Domain.Events
{
    public class ProdutoEventHandler : INotificationHandler<ProdutoAbaixoEstoqueEvent>
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoEventHandler(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task Handle(ProdutoAbaixoEstoqueEvent notification, CancellationToken cancellationToken)
        {
            //manda email, sms, faz tudo oq quiser
            var produto = await _produtoRepository.ObterPorId(notification.AggregateId);
            //aquisição de mais produto deste tipo
        }
    }
}