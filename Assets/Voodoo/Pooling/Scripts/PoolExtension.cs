namespace Voodoo.Pattern
{
    public static class PoolExtension
    {
        public static void Free<T>(this T item) where T : class => GlobalPool<T>.Free(item);
        public static void DisposeOf<T>(this T item) where T : class => GlobalPool<T>.DisposeOf(item);
    }
}
