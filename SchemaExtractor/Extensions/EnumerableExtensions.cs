using System;
using System.Collections.Generic;
using System.Linq;

namespace SchemaExtractor.Extensions
{
    public static class EnumerableExtensions
    {
        public static HashSet<TElement> ToHashSet<TSource, TElement>(this IEnumerable<TSource> source, Func<TSource, TElement> elementSelector, IEqualityComparer<TElement> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

            return new HashSet<TElement>(source.Select(elementSelector), comparer);
        }
    }
}
