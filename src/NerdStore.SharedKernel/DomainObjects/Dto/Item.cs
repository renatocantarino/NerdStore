using System;

namespace NerdStore.SharedKernel.DomainObjects.Dto
{
    public class Item
    {
        public Guid Id { get; set; }

        public int Quantidade { get; set; }
    }
}