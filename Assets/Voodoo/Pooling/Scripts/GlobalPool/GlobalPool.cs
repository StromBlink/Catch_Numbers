namespace Voodoo.Pattern
{
    public static class GlobalPool<T> where T : class
    {
        static Pool<T> _pool;

        static IPoolableCycle<T> _poolableCycle;
        public static IPoolableCycle<T> PoolableCycle
        {
            set
            {
                _poolableCycle = value;
                _pool?.Dispose();
                _pool = PoolManager.GetPool(value);
            }
        }

        public static T Get()
        {
            return _pool?.Get();
        }
        public static void Free(T item) => _pool?.Free(item);
        public static void FreeAll() => _pool?.FreeAll();
        public static void DisposeOf(T item) => _pool?.DisposeOf(item);
        public static void Dispose()
        {
            if (_pool == null)
            {
                return;
            }

            _pool.Dispose();
            _pool = PoolManager.GetPool(_poolableCycle);
        }
    }
}
