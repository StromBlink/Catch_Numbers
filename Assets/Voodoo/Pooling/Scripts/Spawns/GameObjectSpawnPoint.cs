using UnityEngine;

namespace Voodoo.Pattern
{
    public class GameObjectSpawnPoint : SpawnPoint<GameObject>
    {
        protected override void Awake()
        {
            _pool = PoolManager.GetGameObjectPool(prefab, root);
            _pool.maxCount = itemMaxCount;
        }
    }
}