
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using static Utilites;


public class SpawnEditor : MonoBehaviour
{
    public static SpawnEditor Instance;
    [SerializeField] enum PrefabState { Cube, Enemy, Area, DesignObject }

    [SerializeField] PrefabState state;
    [Header("Create Prefab")]
    [SerializeField] GameObject Prefab;

    [SerializeField][Range(0, 100)] int PrefabQuantity;
    [Header("Cube")]
    public GameObject ParentnewCube;
    public GameObject ParentFreeCube;
    public List<GameObject> inactiveCubes = new List<GameObject>(100);
    Vector3 randomposition;
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        switch (state)
        {
            case PrefabState.Cube:

                CreatePrefab();
                StartCoroutine(LoopControlSpawn());

                break;

            case PrefabState.Enemy: SpawnPrefab(PrefabState.Enemy); break;
            case PrefabState.Area: CreatePrefab(); SpawnPrefab(PrefabState.Area); break;
            case PrefabState.DesignObject: CreatePrefab(); SpawnPrefab(PrefabState.DesignObject); break;
        }
    }


    void CreatePrefab()
    {
        for (int i = 0; i < PrefabQuantity; i++)
        {
            GameObject newCube = Instantiate(Prefab);
            newCube.SetActive(false);
            inactiveCubes.Add(newCube);
            newCube.transform.parent = ParentnewCube.transform;
        }
    }
    void SpawnPrefab(PrefabState state)
    {
        for (int i = 0; i < PrefabQuantity; i++)
        {
            randomposition = new Vector3(Random.Range(-48f, 48f), 0.5f, Random.Range(-48f, 48f));
            GameObject prefobject = inactiveCubes[i];
            prefobject.SetActive(true);
            prefobject.transform.position = randomposition;

            if (state == PrefabState.Enemy) { _GameManager.enemyList.Add(inactiveCubes[i]); }
            if (state == PrefabState.Area) { _GameManager.areaList.Add(inactiveCubes[i]); }
        }
    }
    void SpawnCupe()
    {
        if (ParentFreeCube.transform.childCount < 5)
        {
            int count = 100 - ParentFreeCube.transform.childCount;
            for (int i = 0; i < count; i++)
            {

                GameObject cube = inactiveCubes[0];
                CubeEditor cubeEditor = cube.GetComponent<CubeEditor>();
                randomposition = new Vector3(Random.Range(-48f, 48f), 0.5f, Random.Range(-48f, 48f));
                cube.SetActive(true);
                cube.GetComponent<Collider>().isTrigger = false;
                cube.transform.parent = ParentFreeCube.transform;
                inactiveCubes.Remove(cube);
                cube.transform.localScale = new Vector3(.1f, .1f, .1f);
                cube.transform.position = randomposition;
                cube.tag = RandomLevel().ToString();

                cube.transform.DOScale(Vector3.one, 0.3f).OnComplete(() =>
                {
                    cube.transform.DOPunchScale(Vector3.one, 0.3f).OnComplete(() =>
                    {
                        cubeEditor.Start();
                    });
                });
            }
        }
    }
    IEnumerator LoopControlSpawn()
    {
        SpawnCupe();
        yield return new WaitForSeconds(100f);
        yield return StartCoroutine(LoopControlSpawn());
    }
    int RandomLevel()
    {
        int result = 0;
        int random = Random.Range(0, 100);
        if (random < 28) result = 2;
        else if (49 >= random && random >= 28) result = 4;
        else if (68 >= random && random > 49) result = 8;
        else if (84 >= random && random > 68) result = 16;
        else if (100 >= random && random > 84) result = 32;
        return result;

    }

}

