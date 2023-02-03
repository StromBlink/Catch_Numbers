 
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;


public class LeadBoard : MonoBehaviour
{
    public static LeadBoard Instance;
    [Header("Icons")] 
    [SerializeField] private List<Sprite> flagIcons;
    [Header("Player Names")] 
    [SerializeField] private List<string> playerNames;
    
    [Header("Score List")]
    SortedList<int,RectTransform> _listraTransforms=new SortedList<int, RectTransform>(10);
    [SerializeField] private List<Transform> scores;
    [SerializeField] private List<RectTransform> _list = new List<RectTransform>(10);
    List<RectTransform> _position=new List<RectTransform>(10);



    [SerializeField] private GameObject line;


    private Dictionary<int,string> tags;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        /*for (int i = 0; i <  10; i++)
        { 
         _position.Add(_list[i]);
         scores[i].GetChild(0).GetComponent<TMP_Text>().text = playerNames[Random.RandomRange(0, playerNames.Count) ];
         
         /*_list .Add( scores[i].GetComponent<RectTransform>());#1#
         int random_number;
         random_number = Random.RandomRange(0, flagIcons.Count);
         scores[i].GetChild(1).GetComponent<Image>().sprite = flagIcons[random_number];
        }*/

        
    }

    public void ListLine()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            /*string _tag=transform.GetChild(i).GetChild(2).GetComponent<TMP_Text>().text;
            int mynumber=2;
            if (int.TryParse(_tag, out mynumber))
              mynumber  */
        }
       
    }
    public GameObject AddScroLine(  )
    {  
        int random_number;
        
        GameObject temp = Instantiate(line);
        temp.transform.parent = transform;
        
        random_number = Random.RandomRange(0, playerNames.Count);
        temp.transform.GetChild(0).GetComponent<TMP_Text>().text = playerNames[ random_number ];
       
        random_number = Random.RandomRange(0, flagIcons.Count);
        temp.transform.GetChild(1).GetComponent<Image>().sprite = flagIcons[random_number];
        return temp;
    }

    void deneme()
    {
        for (int i = 0; i <  GameManager.Instance.enemyList.Count; i++)
        {if (GameManager.Instance.enemyList[i].transform.childCount>0)
            {
              string  tag=  GameManager.Instance.enemyList[i].transform.GetChild(0).tag;
          
               tags.Add(GameManager.Instance.enemyList[i].GetInstanceID(),tag);
            }
        }
    }
    private void Update()
    {  
        /*int tag;
        
        for (int i = 0; i <  GameManager.Instance.enemyList.Count; i++)
        {
            if (GameManager.Instance.enemyList[i].transform.childCount>0)
            {
                tag=CollisionManager.Instance.ToInt(GameManager.Instance.enemyList[i].transform.GetChild(0));
          
                if (!_listraTransforms.ContainsKey(tag))
                    _listraTransforms.Add(tag,_list[i]);
            }
        } 
        testlist();
        _listraTransforms.Clear();*/

    }

    void testlist()
    {
        if(_listraTransforms.Count!=1)
            for (int i = 0; i <_listraTransforms.Count; i++)
            {
                _listraTransforms.Values[i].position = _position[i].position;
                
                if (_list[i].position == _listraTransforms.Values[i].position  )
                {
                    _list[i].position = _position[i+1].position;
                }

                
            }
        
    }

    void ListedBoard()
    {
        if(_listraTransforms.Count!=1)
            for (int i = 0; i <_listraTransforms.Count; i++)
            { 
                _listraTransforms.Values[i].position = _position[i].position;
                
                for (int j = 0; j <_listraTransforms.Count; j++)
                {
                    if (_listraTransforms.Values[j].position==_listraTransforms.Values[i].position && i!=j)
                    {
                        for (int k = i; k < _list.Count; k++)
                        { 
                            if (i<_list.Count)
                            {
                                _list[j].position = _position[i+1].position;
                                _listraTransforms.Values[i].position = _position[i].position;
                            }
                            /*else
                            {
                                _listraTransforms.Values[j].position = _position[i1].position;
                            }
                            */
                             
                        }
                         
                         
                    }
                     
                }
            }
    }

    
}
