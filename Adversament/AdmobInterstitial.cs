using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdmobInterstitial : MonoBehaviour
{
    const string interstitialRewardedId = "ca-app-pub-6882468169022717/2655434686";

    private RewardedInterstitialAd rewardedInterstitialAd;
    private Action onSuccess;

    private void AdLoadCallback(RewardedInterstitialAd ad, AdFailedToLoadEventArgs error)
    {
        if (error == null) {
            rewardedInterstitialAd = ad;
        }
    }

    public void Initialize()
    {
        AdRequest request = new AdRequest.Builder().Build();

        RewardedInterstitialAd.LoadAd(interstitialRewardedId, request, AdLoadCallback);
    }

    public void ShowAds(Action onSuccess)
    {
        rewardedInterstitialAd.OnAdDidPresentFullScreenContent += ReloadAd;
        rewardedInterstitialAd.Show((Reward r) => { onSuccess(); });
    }

    private void ReloadAd(object sender, EventArgs e)
    {
        Initialize();
    }
}
