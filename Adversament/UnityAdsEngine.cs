using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAdsEngine : AdsEngine, IUnityAdsListener
{
    const string game_id = "4191276";

    private Action OnSuccess;
    private Action OnSkipOrFail;

    public override void Initialize()
    {
        if (Advertisement.isSupported) {
            Advertisement.Initialize(game_id, false);
            Advertisement.AddListener(this);
        }
        else
            Debug.LogError("Ads not suported. by Gleb1000");
    }

    public override void ShowAds(Action OnSuccessView, Action OnSkipOrFail, RewardedType type)
    {
        if (Advertisement.IsReady()) {
            Interactable.Mode cache = Interactable.CurrentMode;
            Interactable.CurrentMode = Interactable.Mode.none;
            Advertisement.Show("rewardedVideo");
            this.OnSuccess = OnSuccessView;
            this.OnSkipOrFail = OnSkipOrFail;
            Interactable.CurrentMode = cache;
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        NoAds = true;
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (showResult == ShowResult.Finished)
            OnSuccess();
        else OnSkipOrFail?.Invoke();
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        Debug.Log("video started");
    }

    public void OnUnityAdsReady(string placementId)
    {
        NoAds = false;
    }
}
