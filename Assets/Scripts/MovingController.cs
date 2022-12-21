using UnityEngine;

public class MovingController : MonoBehaviour
{
    public static MovingController Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Moving(float speed, Transform transform)
    {
        Vector3 movingSpeed = Vector3.forward * Time.deltaTime * speed;
        Vector3 position = new Vector3(transform.localPosition.x, 0.5f, transform.localPosition.z);
        transform.Translate(movingSpeed);
    }
    public void Roration(float speed, GameObject gameObject)
    {
        gameObject.transform.rotation = Quaternion.Euler(0, speed, 0);
    }

    public void Follow(Transform _Chaser_gameobject, Transform targetTransform, float speed, float targetDistance)
    {
        Vector3 displacementFromTarget = targetTransform.position - _Chaser_gameobject.transform.position;
        Vector3 directionToTarget = displacementFromTarget.normalized;
        Vector3 velocity = directionToTarget * speed;

        float distanceToTarget = displacementFromTarget.magnitude;

        if (distanceToTarget > targetDistance)
        {
            /* _Chaser_gameobject.transform.Translate(velocity * Time.deltaTime); */
            _Chaser_gameobject.transform.position = (Vector3.Lerp(_Chaser_gameobject.transform.position, targetTransform.position, Time.deltaTime * speed));
            _Chaser_gameobject.transform.rotation = Quaternion.Slerp(Quaternion.LookRotation(displacementFromTarget), transform.rotation, Time.deltaTime * speed);
        }

    }
}
