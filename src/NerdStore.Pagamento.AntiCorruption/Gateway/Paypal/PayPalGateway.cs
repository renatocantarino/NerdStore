using System;
using System.Linq;

namespace NerdStore.Pagamento.AntiCorruption.Gateway.Paypal
{
    public class PayPalGateway : IPayPalGateway
    {
        public bool CommitTransaction(string cardHashKey, string orderId, decimal ammount) => new Random().Next(2) == 0;

        public string GetCardHashKey(string serviceKey, string cartaoCredito) => new(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 10)
                                                                           .Select(s => s[new Random().Next(s.Length)]).ToArray());

        public string GetPayPalServiceKey(string apiKey, string encriptKey) => new(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 10)
                                                                          .Select(s => s[new Random().Next(s.Length)]).ToArray());
    }
}