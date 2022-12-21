using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Linq;
using static Utilites;


public enum IntruderState { inside, outside }
public enum FreeState { yes, enemy_1, enemy_2 }


public class AreaManager : MonoBehaviour
{

    IntruderState intruderState;
    FreeState freeState;
    [Header("Area RefObjects")]
    [SerializeField] GameObject Castle;
    [SerializeField] ParticleSystem[] p_system;
    [SerializeField] float areaCaptureCountDown = 0;

    public static float createCubeCountDown;
    [SerializeField] Image areaCapture_Fill;
    [SerializeField] GameObject prefabCube;
    Vector3 newScale;
    public float range = 3f;
    GameObject target;
    float getInstanceID;
    bool oncapture;
    bool onanimaton = true;
    float _countDown;

    private void Start()
    {

        ParticulControl(false);
        newScale = new Vector3(range / 3.5f, range / 3.5f, range / 3.5f);
        transform.localScale = newScale;
        InvokeRepeating("UpdateTarget", 0f, 0.25f);
        StartCoroutine(SpawnNumber(target));
    }


    void Update()
    {
        if (freeState == FreeState.enemy_2)
        {
            if (areaCapture_Fill.fillAmount < 1)
            {
                areaCapture_Fill.fillAmount = _countDown / areaCaptureCountDown;
                _countDown -= Time.deltaTime;
                if (_countDown < 0) { _countDown = 0; }

            }
            if (target != null) _countDown += Time.deltaTime * 2;
        }
        if (oncapture) return;
        if (areaCapture_Fill.fillAmount < 1)
        {
            areaCapture_Fill.fillAmount = _countDown / areaCaptureCountDown;
            _countDown -= Time.deltaTime;
            if (_countDown < 0) { _countDown = 0; }

        }
        if (target != null && intruderState == IntruderState.inside) _countDown += Time.deltaTime * 2;

    }
    void UpdateTarget()
    {
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in _GameManager.enemyList)
        {
            if (enemy.transform.childCount > 0)
            {
                GameObject targetCube = enemy.transform.GetChild(0).gameObject;
                float distanceToEnemy = Vector3.Distance(transform.position, targetCube.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = targetCube;
                }
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            intruderState = IntruderState.inside;


            /*  target = nearestEnemy; */
            CaptureArea(nearestEnemy);
            IntruderClour(nearestEnemy);
            StartCoroutine(Animation());

        }
        else
        {
            intruderState = IntruderState.outside;

            if (oncapture) return;

            StopCoroutine(Animation());
            IntruderClour(target);
            target = null;
        }

    }
    void CaptureArea(GameObject other)
    {
        if (intruderState == IntruderState.inside)
        {
            if (freeState == FreeState.yes)
            {
                ParticulControl(true);
                target = other.gameObject;
                oncapture = true;
                areaCapture_Fill.fillAmount = 1;
                areaCapture_Fill.color = other.GetComponent<MeshRenderer>().material.color;
                getInstanceID = other.GetInstanceID();
                freeState = FreeState.enemy_1;
                _countDown = 3f;
            }
            if (getInstanceID != other.GetInstanceID() && target != null && freeState == FreeState.enemy_1)
            {
                areaCapture_Fill.fillAmount = 0;
                _countDown = 0;
                freeState = FreeState.enemy_2;
            }

            if (areaCapture_Fill.fillAmount >= 1 && (!oncapture || freeState == FreeState.enemy_2))
            {
                getInstanceID = other.GetInstanceID();
                target = other.gameObject;
                ParticulControl(true);
                freeState = FreeState.enemy_1;
            }
        }
    }

    IEnumerator Animation()
    {
        if (intruderState == IntruderState.outside) yield break;

        if (onanimaton)
        {
            transform.DOPunchScale(Vector3.one * -0.05f, 0.1f).OnComplete(() => { transform.localScale = newScale; });
            onanimaton = false;
        }

        yield return new WaitForSeconds(1f);
        onanimaton = true;
        if (!oncapture || freeState == FreeState.enemy_2) StartCoroutine(Animation());
    }
    void SpawnCube(GameObject other, GameObject instantiate)
    {
        other.GetComponent<CollisionManager>().CollisionDetection(instantiate);
    }
    IEnumerator SpawnNumber(GameObject other)
    {
        float areadistance = 3f;

        if (target != null && oncapture)
        {
            GameObject cubesoldier = Instantiate(prefabCube, transform.position, Quaternion.identity);
            float distance = Vector3.Distance(other.transform.position, cubesoldier.transform.position);
            cubesoldier.transform.DOMove(other.transform.position, distance / 25f).SetEase(Ease.Linear).OnComplete(() =>
            {

                float distance = Vector3.Distance(other.transform.position, cubesoldier.transform.position);
                if (distance < areadistance) { SpawnCube(other, cubesoldier); }
                else
                    cubesoldier.transform.DOMove(other.transform.position, distance / 20f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        float distance = Vector3.Distance(other.transform.position, cubesoldier.transform.position);
                        if (distance < areadistance) { SpawnCube(other, cubesoldier); }
                        else
                            cubesoldier.transform.DOMove(other.transform.position, distance / 15f).SetEase(Ease.Linear).OnComplete(() =>
                            {
                                float distance = Vector3.Distance(other.transform.position, cubesoldier.transform.position);
                                if (distance < areadistance) { SpawnCube(other, cubesoldier); }
                                else
                                    cubesoldier.transform.DOMove(other.transform.position, distance / 10f).SetEase(Ease.Linear).OnComplete(() =>
                                {
                                    float distance = Vector3.Distance(other.transform.position, cubesoldier.transform.position);
                                    if (distance < areadistance) { SpawnCube(other, cubesoldier); }
                                    else
                                        cubesoldier.transform.DOMove(other.transform.position, distance / 10f).SetEase(Ease.Linear).OnComplete(() =>
                                        { SpawnCube(other, cubesoldier); });
                                });
                            });
                    });
            });

        }
        float newcountdown = Random.Range(createCubeCountDown - 1, createCubeCountDown + 1);
        yield return new WaitForSeconds(newcountdown);
        StartCoroutine(SpawnNumber(target));
    }
    void IntruderClour(GameObject other)
    {
        if (intruderState == IntruderState.inside)
            AreaColor(other.GetComponent<MeshRenderer>().material.color);
        else AreaColor(Color.gray);

    }
    void AreaColor(Color newColor)
    {
        areaCapture_Fill.color = newColor;
        Transform t = Castle.transform;
        t.GetChild(0).GetComponent<MeshRenderer>().material.color = newColor;
        t.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("Shadow Color", newColor);

        t.GetChild(1).GetComponent<Image>().color = new Color(newColor.r, newColor.g, newColor.b, 0.3f);
        t.GetChild(2).GetComponent<Image>().color = newColor;
        t.GetChild(3).GetComponent<Image>().color = newColor;

    }
    void AddList(Transform other, Transform this__)
    {
        other.GetComponent<CollisionManager>().listFreeCubes.Add(this__);
    }


    void ParticulControl(bool key)
    {
        foreach (var item in p_system)
        {
            if (!key) item.Stop();
            if (key) item.Play();
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
