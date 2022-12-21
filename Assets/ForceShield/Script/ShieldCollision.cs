using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using static Utilites;

public class ShieldCollision : MonoBehaviour
{

    [SerializeField] string[] _collisionTag;
    [SerializeField] GameObject Player;
    [SerializeField] TMP_Text Shield;
    [SerializeField] Slider RepeatEnergy;
    [SerializeField] float healt;
    [SerializeField] bool player;
    float damage;

    float hitTime;
    Material mat;

    void Start()
    {
        Healt();
        if (GetComponent<Renderer>())
        {
            mat = GetComponent<Renderer>().sharedMaterial;
        }

    }
    public void Healt()
    {
        Transform parent = Player.transform;
        healt = 0;
        for (int i = 0; i < parent.childCount; i++)
        {
            healt = healt + _CollisionManager.ToInt(parent.GetChild(i));
            damage = 0;
        }

    }
    void FindHealtCube()
    {
        Transform parent = Player.transform;
        for (int i = 0; i < parent.childCount; i++)
        {
            if (_CollisionManager.ToInt(parent.GetChild(i)) < damage)
            {
                Transform child = parent.GetChild(i);
                child.DOPunchScale(new Vector3(-.1f, -.1f, -.1f), 0.1f).OnComplete(() =>
                {
                    child.DOScale(new Vector3(.1f, .1f, .1f), .3f).OnComplete(() =>
                     {
                         SpawnEditor.Instance.inactiveCubes.Add(child.gameObject);
                         child.parent = _SpawnEditor.ParentnewCube.transform;
                         child.gameObject.SetActive(false);
                         /*  transform.parent.GetComponent<CollisionManager>().listCatchCubes.Remove(_CollisionManager.ToInt(child)); */
                     });
                });
            }
        }
    }
    void TextShield() { Shield.text = healt.ToString(); RepeatEnergy.value = -hitTime; }
    void ShieldSizeController()
    {

        if (healt < 0) healt = 0;
        float x = Mathf.Abs(healt * 0.3f);
        x = Mathf.Sqrt(x);

        transform.localScale = new Vector3(x, x, x);
    }

    void Update()
    {
        if (player) TextShield();
        ShieldSizeController();
        FindHealtCube();


        if (hitTime > 0)
        {
            float myTime = Time.fixedDeltaTime * 100;
            hitTime -= myTime;
            if (hitTime < 0)
            {
                hitTime = 0;
            }
            mat.SetFloat("_HitTime", hitTime);
        }
        else
        {
            print("hittime<0");
            Healt();

        }

    }

    void OnCollisionEnter(Collision collision)
    {
        for (int i = 0; i < _collisionTag.Length; i++)
        {

            if (_collisionTag.Length > 0 && collision.transform.CompareTag(_collisionTag[i]))
            {
                Destroy(collision.gameObject, 0.1f);
                // Debug.Log("hit");
                ContactPoint[] _contacts = collision.contacts;
                for (int i2 = 0; i2 < _contacts.Length; i2++)
                {
                    healt--;
                    damage++;
                    mat.SetVector("_HitPosition", transform.InverseTransformPoint(_contacts[i2].point));
                    hitTime = 500;
                    mat.SetFloat("_HitTime", hitTime);
                }
            }
        }
    }
}

