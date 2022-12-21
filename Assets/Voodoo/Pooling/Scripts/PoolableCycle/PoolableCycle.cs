using System;

namespace Voodoo.Pattern
{
    public class PoolableCycle<T> : IPoolableCycle<T>
    {
        Func<T> create;
        Action<T> use;
        Action<T> free;
        Action<T> dispose;

        public PoolableCycle(Func<T> create, Action<T> use, Action<T> free, Action<T> dispose)
        {
            this.create = create;
            this.use = use;
            this.free = free;
            this.dispose = dispose;
        }

        public T Create()
        {
            if (create == null)
            {
                return default;
            }

            return create.Invoke();
        }
        public void Use(T item) => use?.Invoke(item);
        public void Free(T item) => free?.Invoke(item);
        public void Dispose(T item) => dispose?.Invoke(item);
    }
}