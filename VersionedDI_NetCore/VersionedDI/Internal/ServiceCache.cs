using System;
using System.Collections.Concurrent;

namespace VersionedDI
{
    internal sealed class ServiceCache : IServiceCache
    {
        private readonly ConcurrentDictionary<object, object> _cachedItems
            = new ConcurrentDictionary<object, object>();

        public object this[object key] => Get(key);

        public object Add(object key, object value)
        {
            if (!_cachedItems.TryAdd(key, value))
            {
                throw new InvalidOperationException("Unable to add item to service cache.");
            }

            return value;
        }

        public object Get(object key)
            => _cachedItems.ContainsKey(key) ? _cachedItems[key] : null;
    }
}
