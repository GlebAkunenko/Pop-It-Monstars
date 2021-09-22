using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AdmobRewardedAds))]
[RequireComponent(typeof(AdmobInterstitial))]
public class AdmobAdsEngine : AdsEngine
{
    private AdmobRewardedAds rewardedAds;
    private AdmobInterstitial interstitial;

    public override void Initialize()
    {
        rewardedAds = GetComponent<AdmobRewardedAds>();
        interstitial = GetComponent<AdmobInterstitial>();

        rewardedAds.Initialize();
        interstitial.Initialize();
    }

    public override void ShowAds(Action onSuccess, Action onSkip, RewardedType rewardedType)
    {
        if (rewardedType == RewardedType.interstitial)
            interstitial.ShowAds(onSuccess);
        else
            rewardedAds.ShowAds(onSuccess, onSkip, rewardedType);
    }
}
