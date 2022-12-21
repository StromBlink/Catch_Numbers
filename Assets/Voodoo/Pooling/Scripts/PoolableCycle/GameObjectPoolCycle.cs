using UnityEngine;

namespace Voodoo.Pattern
{
    public class GameObjectPoolCycle : IPoolableCycle<GameObject>
    {
        public GameObject prefab;
        public Transform parent;

        public GameObjectPoolCycle(GameObject prefab, Transform parent)
        {
            this.prefab = prefab;
            this.parent = parent;
        }

        public GameObject Create() => GameObject.Instantiate(prefab, parent);
        public void Use(GameObject item) => item.SetActive(true);
        public void Free(GameObject item) => item.SetActive(false);

        public void Dispose(GameObject item) => GameObject.Destroy(item);
    }
}