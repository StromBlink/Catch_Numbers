using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static Utilites;


public class TurretController : MonoBehaviour
{
    [SerializeField]
    TurretRotation turretRotation;
    [SerializeField] GameObject Rocket;
    [SerializeField] List<GameObject> enemyList = new List<GameObject>(10);
    void Start()
    {
        enemyList = _GameManager.enemyList.ToList<GameObject>();

        StartCoroutine(FireRate());
    }
    IEnumerator FireRate()
    {
        if (transform.parent != null)
        {
            if (transform.parent.parent != null)
            {
                if (transform.parent.parent.tag == "Player" || transform.parent.parent.tag == "Enemy")
                    DistanceEnemy(transform);

            }


        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(FireRate());

    }
    void DistanceEnemy(Transform my)
    {
        if (enemyList.Contains(transform.parent.parent.gameObject)) { enemyList.Remove(transform.parent.parent.gameObject); }
        for (int i = 0; i < enemyList.Count; i++)
        {
            Transform enemy = enemyList[i].transform;
            for (int x = 0; x < enemy.childCount; x++)
            {
                Vector3 enemyposition = enemy.GetChild(x).transform.position;
                float distanceEnemy = Vector3.Distance(my.position, enemyposition);
                if (distanceEnemy < 10f && enemy.GetChild(x))
                {
                    turretRotation.target = enemy.GetChild(x);
                    FireRocket(enemy.GetChild(x));
                    return;
                }
            }
        }
    }
    void FireRocket(Transform target)
    {
        GameObject rocket = Instantiate(Rocket, transform.position, Quaternion.identity);
        rocket.GetComponent<RocketController>().target = target;
        rocket.GetComponent<Collider>().isTrigger = true;

    }
}
