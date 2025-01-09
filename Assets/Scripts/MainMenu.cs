using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument), typeof(InterstitialLoader))]
public class MainMenu : MonoBehaviour
{
    UIDocument ui;
    VisualElement root;
    Button playButton;
    Button optionsButton;
    Button shopButton;

    AdsInitializer adsInitializer;
    InterstitialLoader interstitialLoader;

    [SerializeField]
    string sceneToLoad;

    [SerializeField]
    OptionsMenu optionsMenu;

    [SerializeField]
    ShopMenu shopMenu;

    [SerializeField]
    BannerPosition _bannerPosition = BannerPosition.BOTTOM_CENTER;
#pragma warning disable CS0414
    [SerializeField]
    string _androidAdUnitId = "Banner_Android";
    [SerializeField]
    string _iOSAdUnitId = "Banner_iOS";
#pragma warning restore CS0414

    private string _adUnitId;

    void Start()
    {
        ui = GetComponent<UIDocument>();
        root = ui.rootVisualElement;
        playButton = root.Q<Button>("play_button");
        optionsButton = root.Q<Button>("options_button");
        shopButton = root.Q<Button>("shop_button");

        playButton.clicked += Play;
        optionsButton.clicked += Options;
        shopButton.clicked += Shop;

        if (PlayerPrefs.GetInt("no_ads", 0) == 0)
        {

            interstitialLoader = GetComponent<InterstitialLoader>();
            interstitialLoader.adLoaded += InterstitialLoaded;

            adsInitializer = GetComponent<AdsInitializer>();

#if UNITY_IOS
            _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
            _adUnitId = _androidAdUnitId;
#endif
            Advertisement.Banner.SetPosition(_bannerPosition);
            adsInitializer.adsInitialized += LoadBanner;
        }
    }

    void Play()
    {
        HideBanner();
        SceneManager.LoadScene(sceneToLoad);
    }

    void Options()
    {
        optionsMenu.Enable();
        root.style.display = DisplayStyle.None;
    }

    void Shop()
    {
        shopMenu.Enable();
        root.style.display = DisplayStyle.None;
    }

    void InterstitialLoaded()
    {
        interstitialLoader.ShowAd();
    }

    public void Enable()
    {
        root.style.display = DisplayStyle.Flex;
    }

    public void LoadBanner()
    {
        BannerLoadOptions options = new()
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };

        Advertisement.Banner.Load(_adUnitId, options);
    }

    void OnBannerLoaded()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu") ShowBanner();
    }

    void OnBannerError(string message)
    {
        Debug.Log($"Banner Error: {message}");
        LoadBanner();
    }

    public void ShowBanner()
    {
        BannerOptions options = new BannerOptions
        {
            clickCallback = OnBannerClicked,
            hideCallback = OnBannerHidden,
            showCallback = OnBannerShown
        };

        Advertisement.Banner.Show(_adUnitId, options);
    }

    public void HideBanner()
    {
        Advertisement.Banner.Hide();
    }

    void OnBannerClicked() { }
    void OnBannerHidden() { }
    void OnBannerShown() { }
}
