using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Utilites;

public class EnemyController : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        //tag bolum levellerine gore degismetedir.
        if (PlayerPrefs.GetInt("MissionLevel", 1) <= 1) transform.tag = "2";
        else transform.tag = (Mathf.Pow(2, PlayerPrefs.GetInt("MissionLevel", 1))).ToString();
        //Ekranda hedefler indikatorleri icin
        _UIManager.AddTargetIndicator(this.gameObject);

        InvokeRepeating("EnemeySearch", 0, 0.5f);
        StartCoroutine(Agent());
    }
    void EnemeySearch()
    {
        for (int i = 0; i < _GameManager.enemyList.Count; i++)
        {
            if (_GameManager.enemyList[i].transform.childCount > 0)
                if (_GameManager.enemyList[i].transform.GetChild(0).GetInstanceID() != transform.GetInstanceID())
                {
                    float tag = _CollisionManager.ToInt(_GameManager.enemyList[i].transform.GetChild(0));
                    float mytag = _CollisionManager.ToInt(transform);
                    if (mytag > tag)
                    {
                        Vector3 enemy = _GameManager.enemyList[i].transform.GetChild(0).position;
                        float distance = Vector3.Distance(transform.position, enemy);
                        if (distance < 10 && enemy != null) { navMeshAgent.destination = enemy; }
                    }
                }
        }
    }
    IEnumerator Agent()
    {
        yield return new WaitForSeconds(0.1f);
        //  Vector3 randomposition = new Vector3(Random.Range(-48f, 48f), 0.5f, Random.Range(-48f, 48f));
        int areanumber = Random.Range(0, _GameManager.areaList.Count - 1);
        Vector3 area = _GameManager.areaList[areanumber].transform.position;
        navMeshAgent.destination = area;
        yield return new WaitForSeconds(15f);
        StartCoroutine(Agent());
    }

}
