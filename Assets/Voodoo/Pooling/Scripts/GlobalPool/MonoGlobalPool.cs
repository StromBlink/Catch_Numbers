using UnityEngine;

namespace Voodoo.Pattern
{
    public static class MonoGlobalPool<T> where T : Component
    {
        static Pool<T> _pool;

        static IPoolableCycle<T> _poolableCycle;
        
        public static void Initialize(T prefab, Transform parent) 
        {
            _poolableCycle = new MonoPoolCycle<T>(prefab, parent);
            _pool?.Dispose();
            _pool = PoolManager.GetPool(_poolableCycle);
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
