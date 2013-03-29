using System;

namespace Uncas.BuildPipeline
{
    public static class MaybeMonad
    {
        public static TU Maybe<T, TU>(this T @this, Func<T, TU> get) where T : class
        {
            if (@this == null)
                return default(TU);
            return get(@this);
        }
    }
}