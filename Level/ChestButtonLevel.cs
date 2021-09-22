using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

    [SerializeField]
    private ChestButtonImage[] components;

    private ChestLoot loot;

    private void Start()
    {
        foreach(ChestButtonImage c in components) {
            if (c.Location == MetaSceneDate.GameData.LocationName) {
                c.gameObject.SetActive(true);
                loot = c.Loot;
            }
            else
                c.gameObject.SetActive(false);
        }
    }

    public void Spawn()
    {
        anim.Play();
    }

    protected override void OnFull()
    {
        if (CanOpen) {
            if (adsChest) {
                if (gold)
                    Ads.ShowVideo(() => { OpenChest(); MetaSceneDate.Statistics.AdsChest++; }, OnFailAds, RewardedType.goldChest);
                else
                    Ads.ShowVideo(() => { OpenChest(); MetaSceneDate.Statistics.AdsChest++; }, OnFailAds, RewardedType.optionalSilverChest);
            }
            else OpenChest();
        }
        
        else UpdatePimples();
    }

    private void OpenChest()
    {
        MetaSceneDate.ChestLoot = loot;
        VictoryWindow.self.SaveExperiance();
        SceneManager.LoadScene(chest_open_scene_name);
    }

    private void OnFailAds()
    {
        UpdatePimples();
    }



}
