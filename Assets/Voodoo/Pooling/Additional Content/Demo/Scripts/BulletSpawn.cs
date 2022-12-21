namespace Voodoo.Pattern
{
    public class BulletSpawn : MonoSpawnPoint<Bullet>
    {
        protected override void Spawn()
        {
            Bullet instance = _pool.Get();
            if (instance == null) return;

            instance.freed += () => _pool.Free(instance);
            instance.transform.position = root.position;
        }
    }
}