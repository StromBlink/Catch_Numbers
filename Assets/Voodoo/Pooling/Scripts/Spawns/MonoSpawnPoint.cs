using UnityEngine;

namespace Voodoo.Pattern
{
    public abstract class MonoSpawnPoint<T> : SpawnPoint<T> where T : Component
    {
        protected override void Awake()
        {
            _pool = PoolManager.GetPool(new MonoPoolCycle<T>(prefab, root));
            _pool.maxCount = itemMaxCount;
        }
    }
}