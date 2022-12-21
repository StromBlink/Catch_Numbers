using System;
using System.Collections.Generic;

namespace Voodoo.Pattern
{
    public class Pool<T> : IDisposable 
        where T : class
    {
        bool _isDisposed;

        Stack<T> _unused = new Stack<T>();
        HashSet<T> _pool = new HashSet<T>();

        public int maxCount = -1;

        public int UnusedCount => _unused.Count;
        public bool HasUnused => _unused.Count > 0;
        public int Count => _pool.Count;
        public bool IsFull => maxCount > 0 && _pool.Count >= maxCount;

        IPoolableCycle<T> _adaptor;

        public Pool(IPoolableCycle<T> adaptor) => _adaptor = adaptor;

        public T Get(bool forceCreate = false)
        {
            if (_adaptor == null)
            {
                return default;
            }

            if (IsFull && HasUnused == false)
            {
                return default;
            }

            if (forceCreate == false)
            {
                while (_unused.Count > 0)
                {
                    T item = _unused.Pop();
                    if (item == null)
                    {
                        continue;
                    }

                    _adaptor.Use(item);
                    return item;
                }
            }

            T instance = _adaptor.Create();
            if (instance == null)
            {
                return default;
            }

            _pool.Add(instance);

            return instance;
        }

        public void Free(T item)
        {
            if (item == null)
            {
                return;
            }

            if (_pool.Contains(item) == false)
            {
                return;
            }

            _adaptor.Free(item);
            _unused.Push(item);
        }

        public void FreeAll()
        {
            foreach (T item in _pool)
            {
                if (item == null)
                {
                    continue;
                }

                _adaptor.Free(item);
                _unused.Push(item);
            }

            _pool.Clear();
        }

        public void DisposeOf(T item)
        {
            if (item == null)
            {
                return;
            }

            _adaptor.Dispose(item);

            if (_pool.Contains(item))
            {
                _pool.Remove(item);
            }
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;

            if (_adaptor != null)
            {
                foreach (T item in _pool)
                {
                    _adaptor.Dispose(item);
                }
            }

            _unused.Clear();
            _pool.Clear();

            _adaptor = null;

            GC.SuppressFinalize(this);
        }
    }
}