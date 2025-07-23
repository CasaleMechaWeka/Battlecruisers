using BattleCruisers.Data;
using BattleCruisers.UI.ScreensScene;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class IAPManager : MonoBehaviour, IDetailedStoreListener
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
        builder.AddProduct(small_coin_pack, ProductType.Consumable);
        builder.AddProduct(medium_coin_pack, ProductType.Consumable);
        builder.AddProduct(large_coin_pack, ProductType.Consumable);
        builder.AddProduct(extralarge_coin_pack, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }


    private bool IsInitialized()
    {
        return storeController != null && _StoreExtensionProvider != null;
    }

    //Step 4 modify purchasing
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (args.purchasedProduct.definition.id == premium_version_product) //allowing for just the single IAP at this stage
        {
            DataProvider.GameModel.PremiumEdition = true;
            DataProvider.GameModel.AddBodykit(0);
            DataProvider.SaveGame();
        }
        else if (args.purchasedProduct.definition.id == small_coin_pack)
        {
            BlackMarketScreenController.Instance.purchasedIAP.Invoke(this, new IAPEventArgs() { CoinsPack = small_coin_pack });
        }
        else if (args.purchasedProduct.definition.id == medium_coin_pack)
        {
            BlackMarketScreenController.Instance.purchasedIAP.Invoke(this, new IAPEventArgs() { CoinsPack = medium_coin_pack });
        }
        else if (args.purchasedProduct.definition.id == large_coin_pack)
        {
            BlackMarketScreenController.Instance.purchasedIAP.Invoke(this, new IAPEventArgs() { CoinsPack = large_coin_pack });
        }
        else if (args.purchasedProduct.definition.id == extralarge_coin_pack)
        {
            BlackMarketScreenController.Instance.purchasedIAP.Invoke(this, new IAPEventArgs() { CoinsPack = extralarge_coin_pack });
        }
        else
        {
            Debug.Log(" ===> Purchase Failed ---> " + args.purchasedProduct.definition.id);
        }

        return PurchaseProcessingResult.Complete;
    }

    //**************************** Dont worry about these methods ***********************************
    // We should probably worry about these methods!!!
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
            Debug.Log("RestorePurchaseTrace: RestorePurchases FAIL. Not initialized.");
            return;
        }

        foreach (var product in storeController.products.all)
            Debug.Log($"RestorePurchaseTrace: Product: {product.definition.id}, Type: {product.definition.type}, HasReceipt: {product.hasReceipt}");

        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            Debug.Log("RestorePurchaseTrace: RestorePurchases started for iOS/macOS...");

            var apple = _StoreExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((bool success, string error) =>
            {
                Debug.Log("RestorePurchaseTrace: iOS RestoreTransactions callback - Success: " + success + ", Error: " + error);
                if (success)
                {
                    Debug.Log("RestorePurchaseTrace: iOS restore succeeded, calling ProcessRestoredPurchases...");
                    ProcessRestoredPurchases();
                }
                else
                {
                    Debug.Log("RestorePurchaseTrace: iOS restore failed: " + error);
                }
            });
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            Debug.Log("RestorePurchaseTrace: RestorePurchases started for Android...");

            var android = _StoreExtensionProvider.GetExtension<IGooglePlayStoreExtensions>();
            android.RestoreTransactions((bool success, string error) =>
            {
                Debug.Log("RestorePurchaseTrace: Android RestoreTransactions callback - Success: " + success + ", Error: " + error);
                if (success)
                {
                    Debug.Log("RestorePurchaseTrace: Android restore succeeded, calling ProcessRestoredPurchases...");
                    ProcessRestoredPurchases();
                }
                else
                {
                    Debug.Log("RestorePurchaseTrace: Android restore failed: " + error);
                }
            });
        }
        else
        {
            Debug.Log("RestorePurchaseTrace: RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }

        Debug.Log("RestorePurchaseTrace: === RESTORE PURCHASES END ===");
    }

    private void ProcessRestoredPurchases()
    {
        Debug.Log("RestorePurchaseTrace: Checking " + storeController.products.all.Length + " products for restored purchases...");

        int processedCount = 0;

        foreach (var product in storeController.products.all)
        {
            Debug.Log($"RestorePurchaseTrace: Checking product: {product.definition.id}");
            Debug.Log($"RestorePurchaseTrace:   - Type: {product.definition.type}");
            Debug.Log($"RestorePurchaseTrace:   - HasReceipt: {product.hasReceipt}");
            Debug.Log($"RestorePurchaseTrace:   - AvailableToPurchase: {product.availableToPurchase}");

            if (product.hasReceipt && product.definition.type == ProductType.NonConsumable)
            {
                Debug.Log($"RestorePurchaseTrace: *** FOUND RESTORED NON-CONSUMABLE: {product.definition.id} ***");

                try
                {
                    Debug.Log("RestorePurchaseTrace: Processing restored purchase directly...");

                    if (product.definition.id == premium_version_product)
                    {
                        Debug.Log("RestorePurchaseTrace: Applying Premium Edition...");
                        DataProvider.GameModel.PremiumEdition = true;
                        DataProvider.GameModel.AddBodykit(0);
                        DataProvider.SaveGame();
                        Debug.Log("RestorePurchaseTrace: Premium Edition applied successfully");
                    }
                    else
                    {
                        Debug.Log($"RestorePurchaseTrace: Unknown non-consumable product: {product.definition.id}");
                    }

                    processedCount++;
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"RestorePurchaseTrace: Failed to process restored purchase {product.definition.id}: {e.Message}");
                    Debug.LogError($"RestorePurchaseTrace: Exception type: {e.GetType().Name}");
                    Debug.LogError($"RestorePurchaseTrace: Stack trace: {e.StackTrace}");
                }
            }
            else
            {
                Debug.Log($"RestorePurchaseTrace: Skipping product {product.definition.id} - HasReceipt: {product.hasReceipt}, Type: {product.definition.type}");
            }
        }

        Debug.Log($"RestorePurchaseTrace: === PROCESS RESTORED PURCHASES END - Processed {processedCount} items ===");

        // Show one-line summary on screen
        if (processedCount > 0)
        {
            LogToScreen($"Restore complete: {processedCount} item(s) restored");
        }
        else
        {
            LogToScreen("Restore complete: No items found to restore");
        }
    }

    private void LogToScreen(string message)
    {
        Debug.Log("IAP Restore: " + message);
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
    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {

    }
}