using System;

namespace NerdStore.SharedKernel.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException()
        {
        }

        public DomainException(string mensagem) : base(mensagem)
        {
        }

        public DomainException(string mensagem, Exception innerExpection) : base(mensagem, innerExpection)
        {
        }
    }
}