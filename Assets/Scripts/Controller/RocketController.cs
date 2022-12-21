using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utilites;

public class RocketController : MonoBehaviour
{
    public Transform target;
    [SerializeField] float distance;

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            distance = Vector3.Distance(target.position, transform.position);
            if (distance < 3.5f)
                GetComponent<Collider>().isTrigger = false;
            _MovingController.Follow(transform, target, 2.5f, 0);
        }

    }
}
