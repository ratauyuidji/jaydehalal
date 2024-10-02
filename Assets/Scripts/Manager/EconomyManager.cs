using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance;
    [SerializeField] private TextMeshProUGUI moneyText;
    private int currentMoney;

    private void Awake()
    {
        //PlayerPrefs.DeleteAll();
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Update()
    {
        
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
    public void AssignMoneyText()
    {
        if (moneyText == null)
        {
            moneyText = GameObject.Find("MoneyText").GetComponent<TextMeshProUGUI>();
        }
    }
    public void IncreaseMoney(int money)
    {
        currentMoney += money;
        UpdateMoney();
    }
    public void BuySkin(int cost)
    {
        currentMoney -= cost;
        PlayerPrefs.SetInt("CurrentMoney", currentMoney);
        PlayerPrefs.Save();
        UpdateMoney();
    }

    public void ResetMoney()
    {
        currentMoney = 0;
        UpdateMoney();
    }
    public int GetCurrentMoney()
    {
        return currentMoney;
    }
}
