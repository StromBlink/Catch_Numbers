using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeadBoard : MonoBehaviour
{   
   
    SortedList<int,RectTransform> _listraTransforms=new SortedList<int, RectTransform>(10);
    [SerializeField] private List<RectTransform> _list = new List<RectTransform>(10);
    List<RectTransform> _position=new List<RectTransform>(10);
    private void Start()
    {
        for (int i = 0; i <  10; i++)
        { 
         _position.Add(_list[i]);
         _list[i].GetComponent<TMP_Text>().text = "Player " + (i + 1);
        }
    }
    private void Update()
    {  
        int tag;
        for (int i = 0; i <  GameManager.Instance.enemyList.Count; i++)
        {
            tag=CollisionManager.Instance.ToInt(GameManager.Instance.enemyList[i].transform.GetChild(0));
          
            if (!_listraTransforms.ContainsKey(tag))
                 _listraTransforms.Add(tag,_list[i]);
        } 
      
        if(_listraTransforms.Count!=1)
            for (int i = 0; i <_listraTransforms.Count; i++)
            {
                 _listraTransforms.Values[i].position = _position[i].position;
            }
        _listraTransforms.Clear();
        
    }

    public void Follow(RectTransform _Chaser_gameobject, RectTransform targetTransform, float speed, float targetDistance)
    {
        Vector3 displacementFromTarget = targetTransform.position - _Chaser_gameobject.transform.position;
        Vector3 directionToTarget = displacementFromTarget.normalized;
        Vector3 velocity = directionToTarget * speed;

        float distanceToTarget = displacementFromTarget.magnitude;

        if (distanceToTarget > targetDistance)
        {
            /* _Chaser_gameobject.transform.Translate(velocity * Time.deltaTime); */
            _Chaser_gameobject.transform.position = (Vector3.Lerp(_Chaser_gameobject.transform.position, targetTransform.position, Time.deltaTime * speed));
            }

    }
}
