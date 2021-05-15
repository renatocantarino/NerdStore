namespace NerdStore.Pagamento.AntiCorruption.Configurations
{
    public interface IConfigurationManager
    {
        string GetValue(string node);
    }
}