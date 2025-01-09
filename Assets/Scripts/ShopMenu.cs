using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class ShopMenu : MonoBehaviour, IDetailedStoreListener
{
    UIDocument ui;
    VisualElement root;
    Button removeAdsButton;
    Button restorePurchasesButton;
    Button backButton;

    IStoreController storeController;
    IExtensionProvider extensionProvider;

    [SerializeField]
    MainMenu mainMenu;

    async void Start()
    {
        ui = GetComponent<UIDocument>();
        root = ui.rootVisualElement;
        removeAdsButton = root.Q<Button>("remove_ads_button");
        restorePurchasesButton = root.Q<Button>("restore_purchases_button");
        backButton = root.Q<Button>("back_button");

        removeAdsButton.clicked += RemoveAds;
        restorePurchasesButton.clicked += RestorePurchases;
        backButton.clicked += Back;

        root.style.display = DisplayStyle.None;

        await UnityServices.InitializeAsync();
        InitializePurchasing();
    }

    void InitializePurchasing()
    {
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        IAPConfigurationHelper.PopulateConfigurationBuilder(ref builder, ProductCatalog.LoadDefaultCatalog());
        UnityPurchasing.Initialize(this, builder);
    }

    void RemoveAds()
    {
        storeController.InitiatePurchase("com.jakeyks.phantomlens.removeads");
    }

    void RestorePurchases()
    {
        extensionProvider.GetExtension<IAppleExtensions>().RestoreTransactions((result, error) => {});
    }

    void Back()
    {
        mainMenu.Enable();
        root.style.display = DisplayStyle.None;
    }

    public void Enable()
    {
        root.style.display = DisplayStyle.Flex;
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        storeController = controller;
        extensionProvider = extensions;

        Product removeAdsProduct = controller.products.WithID("com.jakeyks.phantomlens.removeads");

        removeAdsButton.text = $"{removeAdsProduct.metadata.localizedTitle}: {removeAdsProduct.metadata.localizedPriceString}";
    }
    
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError($"{error}");
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.LogError($"{error}: {message}");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        if (purchaseEvent.purchasedProduct.definition.id == "com.jakeyks.phantomlens.removeads")
        {
            PlayerPrefs.SetInt("no_ads", 1);
            PlayerPrefs.Save();
            mainMenu.HideBanner();
        }

        return PurchaseProcessingResult.Complete;
    }
    
    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        
    }
    
    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {

    }
}
