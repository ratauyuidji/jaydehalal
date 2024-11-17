using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkinManager", menuName = "Skin Manager")]
public class SkinManager : ScriptableObject
{
    public static SkinManager Instance;
    [SerializeField] public Skin[] skins;
    private const string Prefix = "Skin_";
    private const string SelectedSkin = "SelectedSkin";

    private void OnEnable()
    {
        //auto unlock first skin
        if (!IsUnlocked(0))
        {
            Unlock(0);
        }
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void SelectSkin(int skinIndex) => PlayerPrefs.SetInt(SelectedSkin, skinIndex);

    public Skin GetSelectedSkin()
    {
        int skinIndex = PlayerPrefs.GetInt(SelectedSkin, 0);
        if (skinIndex >= 0 && skinIndex < skins.Length)
        {
            return skins[skinIndex];
        }
        else
        {
            return null;
        }
    }
    public void UnlockSkin(SkinShopItem skinShopItem)
    {
        if (!IsUnlocked(9))
        {
            Unlock(9);
            Debug.Log("Special skin unlocked ");
            skinShopItem.CheckUnlock();
        }
        else
        {
            Debug.Log("Skin đã được mở khóa trước đó.");
        }
    }
    public void UnlockSkin2(SkinShopItem skinShopItem)
    {
        if (!IsUnlocked(12))
        {
            Unlock(12);
            Debug.Log("Special skin unlocked ");
            skinShopItem.CheckUnlock();
        }
        else
        {
            Debug.Log("Skin đã được mở khóa trước đó.");
        }
    }

    public void Unlock(int skinIndex) => PlayerPrefs.SetInt(Prefix + skinIndex, 1);

    public bool IsUnlocked(int skinIndex) => PlayerPrefs.GetInt(Prefix + skinIndex, 0) == 1;
}
