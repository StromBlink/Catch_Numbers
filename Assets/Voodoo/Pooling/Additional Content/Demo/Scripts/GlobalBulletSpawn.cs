using System.Threading.Tasks;
using UnityEngine;

namespace Voodoo.Pattern
{
    public class GlobalBulletSpawn : MonoBehaviour
    {
        public int itemSpanMilliseconds = 1000;

        public float span = 0.2f;
        float _timer;

        public Transform root;
        public Bullet prefab;

        public bool isPaused;

        protected void Awake() 
        {
            GlobalPool<Bullet>.PoolableCycle = new MonoPoolCycle<Bullet>(prefab, root);
        }
        
        void Start() => _timer = span;

        void Update()
        {
            if (isPaused)
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

        private void Spawn()
        {
            Bullet item = GlobalPool<Bullet>.Get();
            if (item == null) return;

            Transform itemTransform = item.transform;
            itemTransform.parent   = root;
            itemTransform.position = root.position;

            WaitAndFree(item);
        }

        async void WaitAndFree(Bullet item)
        {
            if (itemSpanMilliseconds <= 0)
            {
                return;
            }

            await Task.Delay(itemSpanMilliseconds);
            GlobalPool<Bullet>.Free(item);
        }
    }
}