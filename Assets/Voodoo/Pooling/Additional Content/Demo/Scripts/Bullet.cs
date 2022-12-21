using UnityEngine;

namespace Voodoo.Pattern
{
    public class Bullet : MonoBehaviour
    {
        public float lifeSpan    = 1.0f;
        public float speed       = 5.0f;

        float _timeCount;
        
        Transform _transform;

        public event System.Action freed;

        void Awake() =>_transform = transform;

        void OnEnable() => _timeCount = Time.time;

        void Update()
        {
            _transform.position += Vector3.up * (speed * Time.deltaTime);

            if (Time.time - _timeCount > lifeSpan)
            {
                freed?.Invoke();
                freed = null;
            }
        }
    }
}