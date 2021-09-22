using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InterstitialAdsManager : MonoBehaviour
{
    [SerializeField]
    private int hpCountForShow;
    [SerializeField]
    private int stepForShow;

    [SerializeField]
    private ChestLoot chestLoot;

    public void Init()
    {
        //if (MetaSceneDate.Player.HealthPoints > hpCountForShow)
        //    MetaSceneDate.GameData.InterstitialStep = 0;

        if ((MetaSceneDate.GameData.InterstitialStep + 1) % (stepForShow + 1) == 0) {
            MetaSceneDate.GameData.InterstitialStep = 0;
            MetaSceneDate.InShop = false;
            Debug.Log("start interstitial");
            Ads.ShowVideo(GivePrize, () => { }, RewardedType.interstitial);
        }
    }

    public void GivePrize()
    {
        MetaSceneDate.ChestLoot = chestLoot;
        SceneManager.LoadScene(MetaSceneDate.chest_scene_name);
    }
}
