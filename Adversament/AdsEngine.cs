using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AdsEngine : MonoBehaviour
{
    public abstract void Initialize();

    public abstract void ShowAds(Action onSuccess, Action onSkip, RewardedType rewardedType);

    protected bool NoAds { get => Ads.NoAds; set => Ads.NoAds = value; }
}
