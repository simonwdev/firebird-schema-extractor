using System;
using System.Collections.Generic;

namespace SchemaExtractor.Extensions
{
    public static class DictionaryExtensions
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default)
        {
            if (dictionary == null) 
                throw new ArgumentNullException(nameof(dictionary));

            if (key == null) 
                throw new ArgumentNullException(nameof(key));

            return dictionary.TryGetValue(key, out var value) ? value : defaultValue;
        }
    }
}