﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Caching.Memory;

namespace Garcia.Infrastructure
{
    internal static class MemoryCacheExtentions
    {
        private static readonly Func<MemoryCache, object> GetEntriesCollection = Delegate.CreateDelegate(typeof(Func<MemoryCache, object>),
        typeof(MemoryCache).GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance).GetGetMethod(true),
        throwOnBindFailure: true) as Func<MemoryCache, object>;

        public static IEnumerable GetKeys(this IMemoryCache memoryCache) =>
            ((IDictionary)GetEntriesCollection((MemoryCache)memoryCache)).Keys;

        public static IEnumerable<T> GetKeys<T>(this IMemoryCache memoryCache) =>
            GetKeys(memoryCache).OfType<T>();
    }
}
