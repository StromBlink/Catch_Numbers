
using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using System.Collections;
using static Utilites;

public class CollisionManager : MonoBehaviour
{
    public static CollisionManager Instance;
    [SerializeField] SpawnEditor spawnEditor;
    [SerializeField] float followSpeed;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject FreeCubes;
    [SerializeField] ParticleSystem Expoler;
    [SerializeField] SortedList<int, Transform> listCatchCubes = new SortedList<int, Transform>(20);
    public List<Transform> listFreeCubes = new List<Transform>(20);

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        //  StartCoroutine(RRUpdate());
        Expoler.Stop();

    }
    void Update()
    {
        for (int i = listCatchCubes.Count - 1; i >= 0; i--)
        {
            if (i == listCatchCubes.Count - 1) _MovingController.Follow(listCatchCubes.Values[i], gameObject.transform, followSpeed, .8f);
            else _MovingController.Follow(listCatchCubes.Values[i], listCatchCubes.Values[i + 1], followSpeed, .8f);

        }

    }

    IEnumerator RRUpdate()
    {
        /* for (int i = listCatchCubes.Count - 1; i >= 0; i--)
        {
            if (i == listCatchCubes.Count - 1) _MovingController.Follow(listCatchCubes.Values[i], gameObject.transform, followSpeed, .8f);
            else _MovingController.Follow(listCatchCubes.Values[i], listCatchCubes.Values[i + 1], followSpeed, .8f);
            listCatchCubes.Values[i].gameObject.SetActive(true);
        } */
        for (int i = listCatchCubes.Count - 1; i >= 0; i--)
        {
            if (i == listCatchCubes.Count - 1) listCatchCubes[i].transform.DOMove(gameObject.transform.position
            /* + Vector3.forward * (i + 0.5f) */, 0.3f);
            else
                listCatchCubes[i].transform.DOMove(listCatchCubes[i + 1].transform.position/*  + Vector3.forward * (i + 0.5f) */, 0.3f);
            listCatchCubes.Values[i].gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(0.3f);
        yield return StartCoroutine(RRUpdate());




    }
    private void OnCollisionEnter(Collision other)
    {
        CollisionDetection(other.gameObject);
    }
    public void CollisionDetection(GameObject other)
    {


        if (!other.CompareTag("Plane"))
        {
            int mynumber = ToInt(gameObject.transform);
            int collisionNumber = ToInt(other.transform);

            if (mynumber == collisionNumber)
            {
                LevelUpNumber(gameObject.transform);
                Destroy(other);
            }
            else if (mynumber > collisionNumber)
            {
                other.GetComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().material;
                FreeCubeorCaughtCube(other);
                StartCoroutine(IE_SearchClon(other));
            }
            else if (mynumber < collisionNumber)
            {
                if (other.transform.parent.CompareTag("Enemy")) { Lose(); }
                if (other.transform.parent.CompareTag("Player")) { Lose(); }
            }
        }

    }
    void FreeCubeorCaughtCube(GameObject other)
    {
        if (other.transform.parent != null)
            if (other.transform.parent.CompareTag("Player") || other.transform.parent.CompareTag("Enemy"))
                if (other.transform.parent.GetChild(0) != null)
                    if (other.transform.parent.GetChild(0).TryGetComponent<CollisionManager>(out CollisionManager collisionManager))
                        collisionManager.listCatchCubes.Remove(ToInt(other.transform));
    }
    void Lose()
    {
        if (TryGetComponent<PlayerManager>(out PlayerManager plyr))
            _UIManager.Win(_UIManager.targetIndicators.Count);
        else Death();
    }
    void Death()
    {
        Expoler.Play();

        Destroy(GetComponent<Rigidbody>());
        foreach (var item in listCatchCubes.Values)
        {
            item.GetComponent<Collider>().isTrigger = false;
            item.parent = null;
        }
        listCatchCubes.Clear();
        GetComponent<EnemyController>().CancelInvoke();
        GetComponent<EnemyController>().StopAllCoroutines();
        /*   if (_GameManager.enemyList.Contains(transform.parent.gameObject))
              _GameManager.enemyList.Remove(transform.parent.gameObject); */

        Destroy(GetComponent<NavMeshAgent>());
        Destroy(GetComponent<EnemyController>());
        Destroy(GetComponent<CollisionManager>());
    }
    IEnumerator IE_SearchClon(GameObject other)
    {

        Transform parent = Player.transform;
        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i).tag == other.tag)
            {
                GameObject child = parent.GetChild(i).gameObject;
                /* -- */
                child.transform.parent = null;
                spawnEditor.inactiveCubes.Add(other);
                other.transform.parent = FreeCubes.transform;/* -- */

                listCatchCubes.Remove(ToInt(other.transform));


                other.transform.DOMove(child.transform.position, 0.3f);
                yield return new WaitForSeconds(0.3f);

                /* spawnEditor.inactiveCubes.Add(other);
                other.transform.parent = FreeCubes.transform;

                listCatchCubes.Remove(ToInt(other.transform)); */


                other.SetActive(false);

                LevelUpNumber(child.transform);
                /*  child.transform.parent = null; */
                yield return StartCoroutine(IE_SearchClon(child));
                yield break;
            }
        }

        other.transform.parent = Player.transform;
        AddTransform(other);
        other.layer = gameObject.layer;
    }


    void LevelUpNumber(Transform Catchgameobject)
    {
        int number = ToInt(Catchgameobject);
        UpdateMoneyandExp(number);
        number = number * 2;
        Catchgameobject.tag = number.ToString();
        Catchgameobject.transform.DOPunchScale(Vector3.one, 0.3f).OnComplete(() =>
       {
           Catchgameobject.GetComponent<CubeEditor>().Catch();

       });

    }
    void UpdateMoneyandExp(int number)
    {
        if (TryGetComponent<PlayerManager>(out PlayerManager plyr))
        {
            PlayerPrefs.SetFloat("Exp", PlayerPrefs.GetFloat("Exp") + returnRank(number) * .6f);
            PlayerPrefs.SetFloat("Money", PlayerPrefs.GetFloat("Money") + returnRank(number) * .5f);
        }
    }

    public float returnRank(int tag)
    {
        int number = 1;
        for (int i = 0; i < 20; i++)
        {
            number = number + number;
            if (number == tag)
                return i;
        }
        return 1;
    }
    void AddTransform(GameObject other)
    {
        int mynumber = ToInt(other.transform);
        if (listCatchCubes.ContainsKey(mynumber)) { listCatchCubes.Remove(mynumber); }
        listCatchCubes.Add(mynumber, other.transform);
    }
    public int ToInt(Transform tag)
    {
        string _tag = tag.tag;
        int mynumber;
        if (int.TryParse(_tag, out mynumber))
        { mynumber = int.Parse(_tag); return mynumber; }
        else { Debug.Log(" Basiriz TryPares" + " ==" + _tag); return mynumber = 999999; }


    }
}
