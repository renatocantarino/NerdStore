using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.SharedKernel.Extensions
{
    public static class CollectionExtensions
    {
        public static void ForEach<T>(this IReadOnlyCollection<T> items, Action<T> itemAction)
        {
            foreach (var item in items)
                itemAction(item);
        }
    }
}