using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Pagamento.AntiCorruption.Gateway.Paypal
{
    public interface IPayPalGateway
    {
        string GetPayPalServiceKey(string apiKey, string encriptKey);

        string GetCardHashKey(string serviceKey, string cartaoCredito);

        bool CommitTransaction(string cardHashKey, string orderId, decimal ammount);
    }
}