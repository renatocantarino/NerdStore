using System;
using System.Linq;

namespace NerdStore.Pagamento.AntiCorruption.Configurations
{
    public class ConfigurationManager : IConfigurationManager
    {
        /// <summary>
        /// valor vem de um arquivo de configuração
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public string GetValue(string node) => new(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 10)
                                                                          .Select(s => s[new Random().Next(s.Length)]).ToArray());
    }
}