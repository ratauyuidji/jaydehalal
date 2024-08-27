using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance;
    public TextMeshProUGUI moneyText;
    private int currentMoney;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        currentMoney = PlayerPrefs.GetInt("CurrentMoney", 0); 
        UpdateMoney();
    }

    public void UpdateMoney()
    {
        if (moneyText != null)
        {
            moneyText.text = currentMoney.ToString();
        }

        PlayerPrefs.SetInt("CurrentMoney", currentMoney);
        PlayerPrefs.Save();
    }

    public void IncreaseMoney(int money)
    {
        currentMoney += money;
        UpdateMoney();
    }

    public void ResetMoney()
    {
        currentMoney = 0;
        UpdateMoney();
    }
}
