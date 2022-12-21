using System;
using UnityEngine;

namespace Voodoo.Pattern
{
    public static class PoolManager
    {
        public static Pool<T> GetPool<T>(IPoolableCycle<T> adaptator) where T : class => new Pool<T>(adaptator);
        public static Pool<T> GetPool<T>(Func<T> create, Action<T> use, Action<T> free, Action<T> dispose) where T : class
        {
            return new Pool<T>(new PoolableCycle<T>(create, use, free, dispose));
        }
        public static Pool<T> GetPool<T>(T prefab, Transform parent) where T : Component
        {
            return new Pool<T>(new MonoPoolCycle<T>(prefab, parent));
        }
        public static Pool<GameObject> GetGameObjectPool(GameObject prefab, Transform parent)
        {
            return new Pool<GameObject>(new GameObjectPoolCycle(prefab, parent));
        }
    }
    
}
