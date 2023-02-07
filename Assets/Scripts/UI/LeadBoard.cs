 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


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
    public List<RectTransform> _list = new List<RectTransform>(10);
  [SerializeField]    List<Vector3> position=new List<Vector3>(10);



    [SerializeField] private GameObject line;


    private Dictionary<int,string> tags;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        
        /*InvokeRepeating("deneme", 1, 1);*/
        
        

    } public GameObject AddScroLine(  )
    {  
        int random_number;
        
        GameObject temp = Instantiate(line);
        temp.transform.parent = transform;
        
        random_number = Random.RandomRange(0, playerNames.Count);
        temp.transform.GetChild(0).GetComponent<TMP_Text>().text = playerNames[ random_number ];
       
        random_number = Random.RandomRange(0, flagIcons.Count);
        temp.transform.GetChild(1).GetComponent<Image>().sprite = flagIcons[random_number];
        
        _list.Add(temp.GetComponent<RectTransform>());
        return temp;
    }

    void deneme()
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            for (int j = i+1; j <transform.childCount-1; j++)
            {
                int intag=Int32.Parse(_list[i].tag); 
                int intag_2=Int32.Parse(_list[j].tag); 
               
               
                if (intag<intag_2)
                {
                    _list[i].position = position[j];
                    _list[j].position = position[i];
                    
                }
            }    
        }
    }
   

    

    

    
}
