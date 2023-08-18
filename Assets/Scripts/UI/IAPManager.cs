using BattleCruisers.Data;
using System;
using UnityEngine;
using UnityEngine.Purchasing;


public class IAPManager : MonoBehaviour, IStoreListener
{
    public static IAPManager instance;

    public IStoreController storeController;
    private static IExtensionProvider _StoreExtensionProvider;
    public const string premium_version_product = "premium_version";
    public const string small_coin_pack = "coins100_pack";
    public const string large_coin_pack = "coins1000_pack";
    public const string medium_coin_pack = "coins500_pack";
    public const string extralarge_coin_pack = "coins5000_pack";

    //************************** Adjust these methods **************************************
    public void InitializePurchasing()
    {        
        if (IsInitialized()) { return; }
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(premium_version_product, ProductType.NonConsumable);

        UnityPurchasing.Initialize(this, builder);
    }


    private bool IsInitialized()
    {
        return storeController != null && _StoreExtensionProvider != null;
    }


    //Step 3 Create methods




    //Step 4 modify purchasing
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (args.purchasedProduct.definition.id == premium_version_product)//allowing for just the single IAP at this stage
        {
            IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;
            applicationModel.DataProvider.GameModel.PremiumEdition = true;
            applicationModel.DataProvider.SaveGame();
        }
        if(args.purchasedProduct.definition.id == small_coin_pack)
        {

        }
        if(args.purchasedProduct.definition.id == medium_coin_pack)
        {

        }
        if(args.purchasedProduct.definition.id == large_coin_pack)
        {

        }
        if(args.purchasedProduct.definition.id == extralarge_coin_pack)
        {

        }
        else
        {
            Debug.Log("Purchase Failed");                
        }
        return PurchaseProcessingResult.Complete;
    }


    //**************************** Dont worry about these methods ***********************************
    private void Awake()
    {
        TestSingleton();
    }

    void Start()
    {
        if (storeController == null) { InitializePurchasing(); }
    }

    private void TestSingleton()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);       
    }

    void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = storeController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                storeController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    public void RestorePurchases()
    {
        if (!IsInitialized())
        {
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            Debug.Log("RestorePurchases started ...");

            var apple = _StoreExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((result) => {
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        else
        {
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialized: PASS");
        storeController = controller;
        _StoreExtensionProvider = extensions;
    }


    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }
}