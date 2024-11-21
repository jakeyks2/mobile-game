using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class MainMenu : MonoBehaviour
{
    UIDocument ui;
    VisualElement root;
    Button playButton;
    Button optionsButton;
    Button shopButton;

    [SerializeField]
    string sceneToLoad;

    [SerializeField]
    BannerPosition _bannerPosition = BannerPosition.BOTTOM_CENTER;

    [SerializeField]
    string _androidAdUnitId = "Banner_Android";

    [SerializeField]
    string _iOSAdUnitId = "Banner_iOS";

    private string _adUnitId;

    void Start()
    {
        ui = GetComponent<UIDocument>();
        root = ui.rootVisualElement;
        playButton = root.Q<Button>("play_button");
        optionsButton = root.Q<Button>("options_button");
        shopButton = root.Q<Button>("shop_button");

        playButton.clicked += Play;

#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
#endif
        Advertisement.Banner.SetPosition(_bannerPosition);
        LoadBanner();
    }

    void Play()
    {
        HideBanner();
        SceneManager.LoadScene(sceneToLoad);
    }

    public void LoadBanner()
    {
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };

        Advertisement.Banner.Load(_adUnitId, options);
    }

    void OnBannerLoaded()
    {
        ShowBanner();
    }

    void OnBannerError(string message)
    {
        Debug.Log($"Banner Error: {message}");
        LoadBanner();
    }

    void ShowBanner()
    {
        BannerOptions options = new BannerOptions
        {
            clickCallback = OnBannerClicked,
            hideCallback = OnBannerHidden,
            showCallback = OnBannerShown
        };

        Advertisement.Banner.Show(_adUnitId, options);
    }

    void HideBanner()
    {
        Advertisement.Banner.Hide();
    }

    void OnBannerClicked() { }
    void OnBannerHidden() { }
    void OnBannerShown() { }
}
