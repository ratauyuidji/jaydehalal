using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinShopItem : MonoBehaviour
{
    [SerializeField] private SkinManager skinManager;
    [SerializeField] private int skinIndex;
    [SerializeField] private Button buyButton;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private GameObject equippedIndicator;
    [SerializeField] private GameObject backgroundImage;
    [SerializeField] private ShopManager shopManager;
    [SerializeField] private TextMeshProUGUI skinNameText;

    private Skin skin;

    void Start()
    {
        skin = skinManager.skins[skinIndex];
        GetComponent<Image>().sprite = skin.Head;

        if (skinManager.IsUnlocked(skinIndex))
        {
            buyButton.gameObject.SetActive(false);
            backgroundImage.SetActive(false);
            equippedIndicator.SetActive(false);
        }
        else
        {
            buyButton.gameObject.SetActive(true);
            costText.text = skin.cost.ToString();
            backgroundImage.SetActive(true);
            equippedIndicator.SetActive(false);
        }
        if (skinNameText != null)
        {
            skinNameText.text = skinManager.GetSelectedSkin().nameSkin;
        }
        UpdateEquippedIndicator();
    }

    public void OnSkinPressed()
    {
        if (skinManager.IsUnlocked(skinIndex))
        {
            skinManager.SelectSkin(skinIndex);
            shopManager.UpdateAllEquippedIndicators();

            // Hiển thị tên của skin đã chọn
            if (skinNameText != null)
            {
                skinNameText.text = skin.nameSkin; 
            }
        }
    }

    public void OnBuyButtonPressed()
    {
        int currentMoney = EconomyManager.Instance.GetCurrentMoney();
        if (currentMoney >= skin.cost && !skinManager.IsUnlocked(skinIndex))
        {
            EconomyManager.Instance.BuySkin(skin.cost);
            skinManager.Unlock(skinIndex);
            buyButton.gameObject.SetActive(false);
            skinManager.SelectSkin(skinIndex);
            backgroundImage.SetActive(false);
            shopManager.UpdateAllEquippedIndicators();

            if (skinNameText != null)
            {
                skinNameText.text = skin.nameSkin;
            }
        }
        else
        {
            Debug.Log("Not enough money :(");
        }
    }

    public void UpdateEquippedIndicator()
    {
        if (skinManager.GetSelectedSkin() == skin)
        {
            equippedIndicator.SetActive(true);
        }
        else
        {
            equippedIndicator.SetActive(false);
        }
    }
}
