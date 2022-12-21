using System.Threading.Tasks;
using UnityEngine;

namespace Voodoo.Pattern
{
    public abstract class SpawnPoint<T> : MonoBehaviour 
        where T : class
    {
        public int itemMaxCount = -1;
        public int itemSpanMilliseconds = 1000;

        public float span = 0.2f;
        float _timer;

        public Transform root;
        public T prefab;

        public bool isPaused = false;

        protected Pool<T> _pool;

        protected abstract void Awake();

        void Start() =>_timer = span;

        void Update()
        {
            if (isPaused || _pool == null)
            {
                return;
            }

            _timer -= Time.deltaTime;

            if (_timer <= 0)
            {
                _timer = span;
                Spawn();
            }
        }

        protected virtual void Spawn() 
        {
            T item = _pool.Get();
            if (item == null) return;

            if (item is GameObject go)
            {
                go.transform.position = root.position;
            }
            else if (item is MonoBehaviour mono)
            {
                mono.transform.position = root.position;
            }

            WaitAndFree(item);
        }

        async void WaitAndFree(T item)
        {
            if (itemSpanMilliseconds <= 0)
            {
                return;
            }

            await Task.Delay(itemSpanMilliseconds);
            _pool.Free(item);
        }

        void OnDestroy()
        {
            _pool?.Dispose();
        }
    }
}