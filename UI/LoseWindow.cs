using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoseWindow : Window
{
    private Animator anim;

    [SerializeField]
    private CanDisabledPopButton addHealthByCoin;

    [SerializeField]
    private PopButton addHealthByAds;

    [SerializeField]
    private PopButton exitButton;

    [SerializeField]
    private HealthShower healthShower;

    private bool adsStarted;

    private void Start()
    {
        healthShower.Init();
        addHealthByCoin.Enable = (MetaSceneDate.Player.HealthPoints > 0);

        if (Ads.NoAds)
            addHealthByAds.gameObject.SetActive(false);

        anim = GetComponent<Animator>();
    }

    public void Open()
    {
        anim.SetBool("opened", true);
        addHealthByCoin.Enable = (MetaSceneDate.Player.HealthPoints > 0);
    }

    public override void OnOpen()
    {
        Interactable.CurrentMode = Interactable.Mode.canvas;

        exitButton.OnClick.AddListener(Exit);
        addHealthByAds.OnClick.AddListener(ClickToAds);
        addHealthByCoin.OnClick.AddListener(AddLifeByCoin);

        MetaSceneDate.Win = false;
        adsStarted = false;
    }

    public override void OnClose()
    {
        if (!LevelController.Self.Win && !LevelController.Self.OpenVictoryWindow)
            Interactable.CurrentMode = Interactable.Mode.game;

        exitButton.OnClick.RemoveListener(Exit);
        addHealthByAds.OnClick.RemoveListener(ClickToAds);
        addHealthByCoin.OnClick.RemoveListener(AddLifeByCoin);

        LevelController.Self.TimePause = false;
    }

    private void AddLifeByCoin()
    {
        if (MetaSceneDate.Player.HealthPoints < 1)
            Debug.LogError("HP is 0 or lower!!! By Gleb1000");

        AddLife();
    }

    private void ClickToAds()
    {
        if (!adsStarted)
            Ads.ShowVideo(AddLifeByAds, () => { adsStarted = false; }, Ads.RewardedType.addHp);
        adsStarted = true;
    }

    private void AddLifeByAds()
    {
        MetaSceneDate.Player.HealthPoints++;
        MetaSceneDate.Statistics.AdsHP++;
        AddLife();
    }

    private void Exit()
    {
        if (MetaSceneDate.LevelData.TimeLevel)
            SceneManager.LoadScene(MetaSceneDate.GameData.CurrentLocation.Name);
        else {
            if (LevelController.Self.NotLose) {
                anim.SetBool("opened", false);
                LevelController.Self.StartCoroutine(LevelController.Self.Victory());
            }
            else SceneManager.LoadScene(MetaSceneDate.GameData.CurrentLocation.Name);
        }
    }

    private void AddLife()
    {
        PlayerInfo.Self.AddLife();
        anim.SetBool("opened", false);

        MetaSceneDate.Statistics.AdsHP++;

        LevelController.Self.CheckForWin();
    }

}
