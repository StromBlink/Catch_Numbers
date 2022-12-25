using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using static Utilites;
using DG.Tweening;
using TMPro;
using LayerLab;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("-----------Target Indikator-----------")]
    public Canvas IndikatorCanvas;
    public List<TargetIndicator> targetIndicators = new List<TargetIndicator>(10);
    public Camera MainCamera;
    public GameObject TargetIndicatorPrefab;

    [Header("-----------Win and Lose-----------")]
    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject losePanel;

    [Header("-----------In Game-----------")]
    public TMP_Text gold;
    public TMP_Text earngold;
    public TMP_Text diamond;
    [Header("------State Level-----")]
    public TMP_Text MissionLevel;
    public TMP_Text now_Exp;
    public TMP_Text Need_Exp;
    [SerializeField] Slider exp_bar;
    [Header("------Level Control-----")]
    [SerializeField] float exp;
    [SerializeField] float exp_Factor;
    [SerializeField] float[] level_exp = new float[100];
    private void Awake() => Instance = this;
    private void Start()
    {
        /*  PlayerPrefs.SetFloat("Exp", 10000);
         PlayerPrefs.SetFloat("Money", 0);
         PlayerPrefs.SetFloat("DefaultMoney", 1000); */

        CreatLevel_exp();
        /* UpgradeLevel(); */

        Time.timeScale = 0.01f;

        gold.text = PlayerPrefs.GetFloat("DefaultMoney", 0).ToString("00");
        diamond.text = PlayerPrefs.GetFloat("Diamond", 0).ToString("00");

        InvokeRepeating("Updateindicator", 0, 1f);
    }

    void CreatLevel_exp()
    {
        level_exp[1] = exp;
        for (int i = 1; i < level_exp.Length - 1; i++)
        { level_exp[i + 1] = level_exp[i] * exp_Factor; }

    }
    int i = 0;
    public void UpgradeLevel()
    {
        earngold.text = ((int)PlayerPrefs.GetFloat("Money")).ToString();
        PlayerPrefs.SetInt("MissionLevel", i);
        now_Exp.text = ((int)PlayerPrefs.GetFloat("Exp")).ToString();
        Need_Exp.text = ((int)level_exp[i + 1] - level_exp[i]).ToString();

        exp_bar.minValue = (int)level_exp[i];
        exp_bar.maxValue = (int)level_exp[i + 1] - (int)level_exp[i];
        if (exp_bar.maxValue < (int)PlayerPrefs.GetFloat("Exp"))
            exp_bar.DOValue(exp_bar.maxValue, 0.01f).OnComplete(() =>
           {
               FncUgradeLevel();
           }
           );
        else exp_bar.DOValue((int)PlayerPrefs.GetFloat("Exp"), 0.01f).OnComplete(() =>
           {
               FncUgradeLevel();
           }
           );


    }
    void FncUgradeLevel()
    {
        MissionLevel.text = (PlayerPrefs.GetInt("MissionLevel") + 1).ToString();

        if (exp_bar.maxValue < (int)PlayerPrefs.GetFloat("Exp"))
        {
            PlayerPrefs.SetFloat("Exp", (int)PlayerPrefs.GetFloat("Exp") - exp_bar.maxValue);
            i++;
            UpgradeLevel();
        }

    }

    // Update is called once per frame
    void Updateindicator()
    {
        if (targetIndicators.Count > 0)
        {
            for (int i = 0; i < targetIndicators.Count; i++)
            {
                targetIndicators[i].UpdateTargetIndicator();
            }
        }
        else Win(0);

    }

    public void Win(int EnemyCount)
    {
        PlayerPrefs.SetFloat("DefaultExp", PlayerPrefs.GetFloat("DefaultExp") + PlayerPrefs.GetFloat("Exp", 0));
        PlayerPrefs.SetFloat("DefaultMoney", PlayerPrefs.GetFloat("DefaultMoney") + PlayerPrefs.GetFloat("Money", 0));

        winPanel.SetActive(true);
        UpgradeLevel();

        WinPanel(EnemyCount);
        _PlayerManager.enabled = false;

        Time.timeScale = 0.01f;
    }
    void WinPanel(int EnemyCount)
    {
        if (EnemyCount >= 4) return;
        for (int i = EnemyCount; i < 3; i++)
        {
            winPanel.GetComponent<Panel>().otherPanels[i].SetActive(true);

        }
    }
    public void AddTargetIndicator(GameObject target)
    {
        TargetIndicator indicator = GameObject.Instantiate(TargetIndicatorPrefab, IndikatorCanvas.transform).GetComponent<TargetIndicator>();
        indicator.InitialiseTargetIndicator(target, MainCamera, IndikatorCanvas);
        targetIndicators.Add(indicator);
    }
}
