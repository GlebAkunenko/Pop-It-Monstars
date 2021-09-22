using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdmobRewardedAds : MonoBehaviour
{
    const string testRewardedId = "ca-app-pub-3940256099942544/5224354917";

    [SerializeField]
    private bool useTestAdmobId = true;

    private Action OnSuccess;
    private Action OnSkipOrFail;

    private RewardedAd goldChestAd;
    private RewardedAd addHpAd;
    private RewardedAd optionalChestAd;
    private RewardedInterstitialAd interstitialAd;

    private bool success;
    private bool unmuteMusic;
    private bool reloadingAdmob;
    private bool needToReload;
    private Interactable.Mode cache;

    public void Initialize()
    {
        RequestConfiguration requestConfiguration = new RequestConfiguration.Builder()
        .SetMaxAdContentRating(MaxAdContentRating.PG)
        .build();

        MobileAds.SetRequestConfiguration(requestConfiguration);
        MobileAds.Initialize((initStatus) => { });

        UpdateRewardedObjects();
    }

    private void HandlePaidEvent(object sender, AdValueEventArgs args)
    {
        success = true;
        OnSuccess();
    }

    private void UpdateRewardedObjects()
    {
        goldChestAd = CreateAndLoadRewardedAd(RewardedType.goldChest);
        addHpAd = CreateAndLoadRewardedAd(RewardedType.addHp);
        optionalChestAd = CreateAndLoadRewardedAd(RewardedType.optionalSilverChest);

        if (needToReload && !reloadingAdmob)
            ReloadAdmob();
    }

    public void ShowAds(Action onSuccess, Action onSkip, RewardedType type)
    {
        OnSuccess = onSuccess;
        OnSkipOrFail = onSkip;

        success = false;
        if (type == RewardedType.goldChest) {
            if (goldChestAd.IsLoaded())
                goldChestAd.Show();
        }
        else if (type == RewardedType.addHp) {
            if (addHpAd.IsLoaded())
                addHpAd.Show();
        }
        else if (type == RewardedType.optionalSilverChest) {
            if (optionalChestAd.IsLoaded())
                optionalChestAd.Show();
        }

    }


    public RewardedAd CreateAndLoadRewardedAd(RewardedType type)
    {
        string id = testRewardedId;
        if (!useTestAdmobId) {
            switch (type) {
                case RewardedType.goldChest:
                    id = "ca-app-pub-6882468169022717/3123546425";
                    break;
                case RewardedType.addHp:
                    id = "ca-app-pub-6882468169022717/8265198058";
                    break;
                case RewardedType.optionalSilverChest:
                    id = "ca-app-pub-6882468169022717/2766842254";
                    break;
            }
        }

        RewardedAd rewardedAd = new RewardedAd(id);

        rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        AdRequest request = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(request);

        if (!rewardedAd.IsLoaded())
            needToReload = true;

        return rewardedAd;
    }

    private void ReloadAdmob()
    {
        reloadingAdmob = true;
        needToReload = false;
        Initialize();
        if (needToReload)
            Debug.LogError("ads not ready");
    }

    private void HandleRewardedAdOpening(object sender, EventArgs e)
    {
        if (!BackMusic.self.Audio.mute) {
            BackMusic.self.Audio.mute = true;
            unmuteMusic = true;
        }
        cache = Interactable.CurrentMode;
        Interactable.CurrentMode = Interactable.Mode.none;
    }

    private void HandleRewardedAdClosed(object sender, EventArgs e)
    {
        if (unmuteMusic) {
            BackMusic.self.Audio.mute = false;
            unmuteMusic = false;
        }
        Interactable.CurrentMode = cache;

        if (!success)
            OnSkipOrFail?.Invoke();
        UpdateRewardedObjects();
    }

    private void HandleUserEarnedReward(object sender, Reward e)
    {
        MetaSceneDate.GameData.InterstitialStep--;
        success = true;
        OnSuccess();
    }
}
