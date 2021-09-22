using UnityEngine;
using UnityEngine.Advertisements;
using System;
using GoogleMobileAds.Api;

public class Ads : MonoBehaviour
{
    private static Ads self;

    public static bool NoAds { get; set; }

    [SerializeField]
    private AdsEngine engine;

    public void Init()
    {
        if (self != null)
            return;

        self = this;
        DontDestroyOnLoad(gameObject);

        engine.Initialize();
    }

    public static void ShowVideo(Action OnSuccessView, Action OnSkipOrFail, RewardedType type)
    {
        self.engine.ShowAds(OnSuccessView, OnSkipOrFail, type);
    }
}


public enum RewardedType
{
    goldChest,
    addHp,
    optionalSilverChest,
    interstitial
}
