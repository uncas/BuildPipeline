using System;
using System.Collections;
using System.Linq;

namespace Uncas.BuildPipeline
{
    public static class MaybeMonad
    {
        public static TU Maybe<T, TU>(this T item, Func<T, TU> get)
        {
            return Maybe(item, get, () => default(TU));
        }

        public static TU Maybe<T, TU>(this T item, Func<T, TU> get, Func<TU> fallback)
        {
            if (Equals(item, default(T)))
                return fallback();
            return get(item);
        }

        public static TU Maybe<T, TU>(this IEnumerable items, Func<T, TU> get,
                                      Func<TU> fallback)
        {
            return items.OfType<T>().FirstOrDefault().Maybe(get, fallback);
        }

        public static TU Maybe<T, TU>(this IEnumerable items, Func<T, TU> get)
        {
            if (items == null)
                return default(TU);
            return items.OfType<T>().FirstOrDefault().Maybe(get);
        }
    }
}