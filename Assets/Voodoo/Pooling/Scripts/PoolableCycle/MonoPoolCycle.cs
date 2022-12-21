using UnityEngine;

namespace Voodoo.Pattern
{
    public class MonoPoolCycle<T> : IPoolableCycle<T> where T : Component
    {
        public T prefab;
        public Transform parent;

        public MonoPoolCycle(T prefab, Transform parent)
        {
            this.prefab = prefab;
            this.parent = parent;
        }

        public T Create()
        {
            if (prefab != null || parent!=null)
            {
                return Object.Instantiate<T>(prefab, parent);
            }
            return null;
        }

        public void Use(T item)
        {
            if (item != null)
            {
                item.gameObject.SetActive(true);
            }
        }

        public void Free(T item)
        {
            if (item != null)
            {
                item.gameObject.SetActive(false);
            }
        }

        public void Dispose(T item)
        {
            if (item != null)
            {
                Object.Destroy(item.gameObject);
            }
        }
    }
}