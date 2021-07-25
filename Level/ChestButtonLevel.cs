using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChestButtonLevel : Popit
{
    const string chest_open_scene_name = "ChestOpening";

    public bool CanOpen { get; set; } = false;

    [SerializeField]
    private bool gold;
    [SerializeField]
    private bool adsChest;

    [SerializeField]
    private Animation anim;

    public void Spawn()
    {
        anim.Play();
    }

    protected override void OnFull()
    {
        if (CanOpen) {
            if (adsChest) {
                if (gold)
                    Ads.ShowVideo(() => { OpenChest(); MetaSceneDate.Statistics.AdsChest++; }, OnFailAds, Ads.RewardedType.goldChest);
                else
                    Ads.ShowVideo(() => { OpenChest(); MetaSceneDate.Statistics.AdsChest++; }, OnFailAds, Ads.RewardedType.optionalSilverChest);
            }
            else OpenChest();
        }
        
        else UpdatePimples();
    }

    private void OpenChest()
    {
        MetaSceneDate.GoldChest = gold;
        VictoryWindow.self.SaveExperiance();
        SceneManager.LoadScene(chest_open_scene_name);
    }

    private void OnFailAds()
    {
        UpdatePimples();
    }


}
