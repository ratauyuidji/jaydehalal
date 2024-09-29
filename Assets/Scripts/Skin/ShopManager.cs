using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer headRenderer;
    [SerializeField] private SpriteRenderer armRenderer;
    [SerializeField] private SpriteRenderer bodyRenderer;
    [SerializeField] private SkinManager skinManager;

    [SerializeField] private List<SkinShopItem> skinShopItems;

    private void Start()
    {
        UpdateAllEquippedIndicators();
    }
    void Update()
    {
        Skin selectedSkin = skinManager.GetSelectedSkin();
        if (selectedSkin != null)
        {
            headRenderer.sprite = selectedSkin.Head;
            armRenderer.sprite = selectedSkin.Arm;
            bodyRenderer.sprite = selectedSkin.Body;
        }
    }
    public void UpdateAllEquippedIndicators()
    {
        foreach (var item in skinShopItems)
        {
            item.UpdateEquippedIndicator();
        }
    }
}
