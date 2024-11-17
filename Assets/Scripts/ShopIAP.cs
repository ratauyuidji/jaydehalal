using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Purchasing;
using Unity.Services.Core;
using Unity.Services.Authentication;
using UnityEngine.Purchasing.Extension;

[Serializable]
public class ConsumableItem
{
    public string name;
    public string id;
    public string description;
    public float price;
}
[Serializable]
public class NonConsumableItem
{
    public string name;
    public string id;
    public string description;
    public float price;
}

public class ShopIAP : MonoBehaviour, IDetailedStoreListener
{
    IStoreController storeController;
    public ConsumableItem cItem;
    public ConsumableItem cItem2;
    public ConsumableItem cItem3;
    public NonConsumableItem ncItem;
    public NonConsumableItem ncItem2;
    public SkinManager skinManager;
    public SkinShopItem skinShopItem;
    public SkinShopItem skinShopItem2;


    private async void Start()
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        SetUpBuilder();
    }
    void SetUpBuilder()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(cItem.id, ProductType.Consumable);
        builder.AddProduct(cItem2.id, ProductType.Consumable);
        builder.AddProduct(cItem3.id, ProductType.Consumable);
        builder.AddProduct(ncItem.id, ProductType.NonConsumable);
        builder.AddProduct(ncItem2.id, ProductType.NonConsumable);

        UnityPurchasing.Initialize(this, builder);
    }
    public void BuyMoneyButton()
    {
        storeController.InitiatePurchase(cItem.id);
    }
    public void BuyMoney2Button()
    {
        storeController.InitiatePurchase(cItem2.id);
    }
    public void BuyMoney3Button()
    {
        storeController.InitiatePurchase(cItem3.id);
    }
    public void BuySkinButton()
    {
        storeController.InitiatePurchase(ncItem.id);
    }
    public void BuySkin2Button()
    {
        storeController.InitiatePurchase(ncItem2.id);
    }
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        print("success");
        storeController = controller;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        print("inotialized failed " + error);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        print("inotialized failed " + error + message);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        print("purchase failed");
    }
    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        print("purchase failed");
    }

    //processing purchase
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        var product = purchaseEvent.purchasedProduct;
        print("purchase complete" + product.definition.id);
        if(product.definition.id == cItem.id)
        {
            if (EconomyManager.Instance != null)
            {
                EconomyManager.Instance.IncreaseMoney(4000);
            }
            else
            {
                Debug.LogError("EconomyManager.Instance is null");
            }
        }
        else if (product.definition.id == cItem2.id)
        {
            EconomyManager.Instance.IncreaseMoney(15000);
        }
        else if (product.definition.id == cItem3.id)
        {
            EconomyManager.Instance.IncreaseMoney(30000);
        }
        else if (product.definition.id == ncItem.id)
        {
            skinManager.UnlockSkin(skinShopItem);
            EconomyManager.Instance.IncreaseMoney(3000);
        }
        else if (product.definition.id == ncItem2.id)
        {
            skinManager.UnlockSkin2(skinShopItem2);
            EconomyManager.Instance.IncreaseMoney(8000);
        }
        return PurchaseProcessingResult.Complete;
    }
    void CheckNonConsumable(string id)
    {
        if(storeController != null)
        {
            var product = storeController.products.WithID(id);
            if(product != null)
            {
                if(product.hasReceipt)
                {
                    //unlock skin or remove ad
                    SkinManager.Instance.UnlockSkin(skinShopItem);
                }
                else
                {
                    //lockskin or ad 
                }
            }
        }
    }
}
