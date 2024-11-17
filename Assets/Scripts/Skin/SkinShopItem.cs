using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Hapiga.Ads;
using Hapiga.Tracking;

public class SkinShopItem : MonoBehaviour
{
    [SerializeField] private SkinManager skinManager;
    [SerializeField] private int skinIndex;
    public Button buyButton;
    [SerializeField] private TextMeshProUGUI costText;
    public GameObject equippedIndicator;
    public GameObject equippedBanner;
    public GameObject backgroundImage;
    public GameObject lockImage;
    [SerializeField] private ShopManager shopManager;
    [SerializeField] private TextMeshProUGUI skinNameText;
    [SerializeField] private RectTransform moneyPanel;
    [SerializeField] private GameObject childGameObject;

    private Skin skin;
    private Vector3 moneyPanelOriginalPosition;

    void Start()
    {
        CheckUnlock();
        UpdateCosDisplay();
        if (skinNameText != null)
        {
            skinNameText.text = skinManager.GetSelectedSkin().nameSkin;
        }
        UpdateEquippedIndicator();
    }
    private void Update()
    {
        UpdateCosDisplay();
    }
    public void CheckUnlock()
    {
        skin = skinManager.skins[skinIndex];
        Image childImage = childGameObject.GetComponent<Image>();
        if (childImage != null)
        {
            childImage.sprite = skin.Head;
        }
        moneyPanelOriginalPosition = moneyPanel.localPosition;

        if (skinManager.IsUnlocked(skinIndex))
        {
            buyButton.gameObject.SetActive(false);
            backgroundImage.SetActive(false);
            lockImage.SetActive(false);
            equippedIndicator.SetActive(false);
            equippedBanner.SetActive(false);
        }
        else
        {
            buyButton.gameObject.SetActive(true);
            costText.text = skin.cost.ToString();
            backgroundImage.SetActive(true);
            lockImage.SetActive(true);
            equippedIndicator.SetActive(false);
            equippedBanner.SetActive(false);
        }
    }
    public void OnSkinPressed()
    {
        if (skinManager.IsUnlocked(skinIndex))
        {
            skinManager.SelectSkin(skinIndex);
            shopManager.UpdateAllEquippedIndicators();

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
            TrackingManager.TrackEvent($"unlock_suit_{skin.nameSkin}");
            buyButton.gameObject.SetActive(false);
            skinManager.SelectSkin(skinIndex);
            backgroundImage.SetActive(false);
            lockImage.SetActive(false);
            shopManager.UpdateAllEquippedIndicators();

            if (skinNameText != null)
            {
                skinNameText.text = skin.nameSkin;
            }
        }
        else
        {
            StartCoroutine(ShakeMoneyPanel());
        }
    }

    public void UpdateEquippedIndicator()
    {
        if (skinManager.GetSelectedSkin() == skin)
        {
            equippedIndicator.SetActive(true);
            equippedBanner.SetActive(true);
        }
        else
        {
            equippedIndicator.SetActive(false);
            equippedBanner.SetActive(false);
        }
    }

    IEnumerator ShakeMoneyPanel()
    {
        float duration = 0.5f;
        float magnitude = 20f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float xOffset = Random.Range(-1f, 1f) * magnitude;
            moneyPanel.localPosition = new Vector3(moneyPanelOriginalPosition.x + xOffset, moneyPanelOriginalPosition.y, moneyPanelOriginalPosition.z);
            yield return null;
        }
        moneyPanel.localPosition = moneyPanelOriginalPosition;
    }
    void UpdateCosDisplay()
    {
        int currentMoney = EconomyManager.Instance.GetCurrentMoney();
        if (currentMoney < skin.cost)
        {
            costText.color = Color.red;
        }
        else
        {
            costText.color = Color.white;
        }
    }
}
