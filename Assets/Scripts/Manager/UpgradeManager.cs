using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using static Utilites;
public class UpgradeManager : MonoBehaviour
{
    [SerializeField] Scriptle_1 upgradeTyp;
    private Button button;
    private float upgradePrice;
    private void Awake() => button = GetComponent<Button>();
    private void Start()
    {
        uprageText(button.tag);
        upragePrice(button.tag);
        PurchaseControl(button.tag);


        AreaManager.createCubeCountDown = upgradeTyp.areaCreatingTime;
        _PlayerManager.boostTime = upgradeTyp.playerSpeed;
    }
    public void onClick_upgradeTyp(string AreUpgradeordPlayer)
    {
        if (!PurchaseControl(AreUpgradeordPlayer)) return;
        setUpgrade(AreUpgradeordPlayer);
        uprageText(AreUpgradeordPlayer);
        upragePrice(AreUpgradeordPlayer);
        PurchaseControl(AreUpgradeordPlayer);
    }
    void setUpgrade(string AreUpgradeordPlayer)
    {
        switch (AreUpgradeordPlayer)
        {
            case "Strength":
                upgradeTyp.playerSpeed = upgradeTyp.playerSpeed + 0.1f;
                _PlayerManager.boostTime = upgradeTyp.playerSpeed;
                break;
            case "Area":
                upgradeTyp.areaCreatingTime = upgradeTyp.areaCreatingTime - 0.1f;
                AreaManager.createCubeCountDown = upgradeTyp.areaCreatingTime;
                break;
        }
        float defaultPrice = PlayerPrefs.GetFloat("DefaultMoney");
        int defaultPrefabLevel = PlayerPrefs.GetInt(AreUpgradeordPlayer + "Level");
        PlayerPrefs.SetFloat("DefaultMoney", defaultPrice - upgradeTyp.price[defaultPrefabLevel]);
        PlayerPrefs.SetInt(AreUpgradeordPlayer + "Level", defaultPrefabLevel + 1);

        _UIManager.gold.text = PlayerPrefs.GetFloat("DefaultMoney").ToString("00");
        _UIManager.diamond.text = PlayerPrefs.GetFloat("Diamond").ToString("00");
    }
    public bool PurchaseControl(string AreUpgradeordPlayer) =>
    button.interactable = (PlayerPrefs.GetFloat("DefaultMoney", 0) >= upgradeTyp.price[PlayerPrefs.GetInt(AreUpgradeordPlayer + "Level")]) ? true : false;

    void uprageText(string AreUpgradeordPlayer)
    {
        button.transform.GetChild(3).GetComponent<TMP_Text>().text = "Lvl." + (PlayerPrefs.GetInt(AreUpgradeordPlayer + "Level") + 1).ToString();
    }
    void upragePrice(string AreUpgradeordPlayer)
    {
        button.transform.GetChild(4).GetComponent<TMP_Text>().text = ((int)upgradeTyp.price[PlayerPrefs.GetInt(AreUpgradeordPlayer + "Level")]).ToString();
    }
}
