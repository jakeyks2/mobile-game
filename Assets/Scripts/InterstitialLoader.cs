using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class InterstitialLoader : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public event Action adLoaded;

#pragma warning disable CS0414
    [SerializeField]
    string _androidAdUnitId = "Interstitial_Android";
    [SerializeField]
    string _iOSAdUnitId = "Interstitial_iOS";
#pragma warning restore CS0414

    string _adUnitId;

    void Start()
    {
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId
#elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
#endif
    }

    public void LoadAd()
    {
        Debug.Log("Loading interstitial ad");
        Advertisement.Load(_adUnitId, this);
    }

    public void ShowAd()
    {
        Debug.Log("Showing interstitial ad");
        Advertisement.Show(_adUnitId, this);
    }

    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        adLoaded();
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.LogError($"Error loading interstitial ad: {error} - {message}");
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.LogError($"Error showing interstitial ad: {error} - {message}");
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState) { }
}
