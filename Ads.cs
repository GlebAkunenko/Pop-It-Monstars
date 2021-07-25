using UnityEngine;
using UnityEngine.Advertisements;
using System;
using GoogleMobileAds.Api;

public class Ads : MonoBehaviour, IUnityAdsListener
{
    const string testRewardedId = "ca-app-pub-3940256099942544/5224354917";

    private static Ads self;

    public static bool NoAds { private set; get; }

    const string game_id = "4191276";

    [SerializeField]
    private bool useTestAdmobId = true;

    private Action OnSuccess;
    private Action OnSkipOrFail;

    private RewardedAd goldChestAd;
    private RewardedAd addHpAd;
    private RewardedAd optionalChestAd;

    private bool success;
    private bool unmuteMusic;
    private Interactable.Mode cache;
   

    public void Init()
    {
        if (self != null)
            return;

        self = this;
        DontDestroyOnLoad(gameObject);

        //UnityInitialize();
        AdmobInitialize();
    }

    public static void ShowVideo(Action OnSuccessView, Action OnSkipOrFail)
    {
        UnityShow(OnSuccessView, OnSkipOrFail);
    }

    public static void ShowVideo(Action OnSuccessView, Action OnSkipOrFail, RewardedType type)
    {
        self.OnSuccess = OnSuccessView;
        self.OnSkipOrFail = OnSkipOrFail;
        self.AdmobShow(type);
    }

    #region Admob code

    private void AdmobInitialize()
    {
        RequestConfiguration requestConfiguration = new RequestConfiguration.Builder()
        .SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.True)
        .SetMaxAdContentRating(MaxAdContentRating.G)
        .build();

        MobileAds.SetRequestConfiguration(requestConfiguration);
        MobileAds.Initialize((initStatus) => { });

        UpdateRewardedObjects();
    }

    private void UpdateRewardedObjects()
    {
        goldChestAd = CreateAndLoadRewardedAd(RewardedType.goldChest);
        addHpAd = CreateAndLoadRewardedAd(RewardedType.addHp);
        optionalChestAd = CreateAndLoadRewardedAd(RewardedType.optionalSilverChest);
    }

    private void AdmobShow(RewardedType type)
    {
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
        return rewardedAd;
    }

    private void HandleRewardedAdOpening(object sender, EventArgs e)
    {
        if (!BackMusic.self.Audio.mute) {
            BackMusic.self.Audio.mute = true;
            unmuteMusic = true;
        }
        self.cache = Interactable.CurrentMode;
        Interactable.CurrentMode = Interactable.Mode.none;
    }

    private void HandleRewardedAdClosed(object sender, EventArgs e)
    {
        if (unmuteMusic) {
            BackMusic.self.Audio.mute = false;
            unmuteMusic = false;
        }
        Interactable.CurrentMode = self.cache;

        if (!success)
            OnSkipOrFail?.Invoke();
        UpdateRewardedObjects();
    }

    private void HandleUserEarnedReward(object sender, Reward e)
    {
        success = true;
        OnSuccess();
    }

    public enum RewardedType
    {
        goldChest,
        addHp,
        optionalSilverChest
    }


    #endregion


    #region Unity code
    private void UnityInitialize()
    {
        if (Advertisement.isSupported) {
            Advertisement.Initialize(game_id, false);
            Advertisement.AddListener(this);
        }
        else
            Debug.LogError("Ads not suported. by Gleb1000");
    }

    private static void UnityShow(Action OnSuccessView, Action OnSkipOrFail)
    {
        if (Advertisement.IsReady()) {
            Interactable.Mode cache = Interactable.CurrentMode;
            Interactable.CurrentMode = Interactable.Mode.none;
            Advertisement.Show("rewardedVideo");
            self.OnSuccess = OnSuccessView;
            self.OnSkipOrFail = OnSkipOrFail;
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

    #endregion
}
