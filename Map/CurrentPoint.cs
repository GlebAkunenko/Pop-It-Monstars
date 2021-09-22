using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class CurrentPoint : Interactable
{
    public static CurrentPoint Self { private set; get; }
    
    public static float PosY
    {
        get { return Self.transform.localPosition.y; }
    }
    
    [SerializeField]
    private int startHealth;
    [SerializeField]
    private HealthShower healthShower;
    [SerializeField]
    private TextMeshProUGUI stars;
    [SerializeField]
    private InterstitialAdsManager interstitialAds;
    [SerializeField]
    private new Transform camera;
    [SerializeField]
    private ChengeLocation goToNextLocation;
    [SerializeField]
    private Guider[] guiders;

    private bool moving;
    private bool ready;
    private SpriteRenderer spriteRenderer;

    private new void Start()
    {
        base.Start();

        Self = this;
        
        Interactable.CurrentMode = Mode.game;

        interstitialAds.Init();

        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (MetaSceneDate.GameData.CurrentLocation.FirstOpen) {
            MetaSceneDate.GameData.CurrentLocation.PointPos = transform.position;
            MetaSceneDate.GameData.CurrentLocation.CamPos = camera.position;
            MetaSceneDate.GameData.CurrentLocation.FirstOpen = false;
            spriteRenderer.color = Color.green;
        }

        transform.position = MetaSceneDate.GameData.CurrentLocation.PointPos;
        camera.position = MetaSceneDate.GameData.CurrentLocation.CamPos;
        if (MetaSceneDate.Win && !MetaSceneDate.OptionalLevel) {
            MetaSceneDate.GameData.CurrentLocation.Moving = true;
        }

        if (MetaSceneDate.Started)
            MetaSceneDate.Statistics.SendDataToFirebase();

        stars.text = MetaSceneDate.GameData.CurrentLocation.AllStars.ToString();

        LevelBar.Self.Init();
        ReviewButton.self.Init();
        healthShower.Init();
        foreach (Guider guider in guiders)
            guider.Init();

        StartCoroutine(Ready());
        StartCoroutine(LateUpdateCompliteLevels());

    }

    private IEnumerator LateFirstStart()
    {
        yield return new WaitForEndOfFrame();
        MetaSceneDate.LevelData = CheckPoint.GetPointByLevel(1).Data;
    }

    private IEnumerator LateUpdateCompliteLevels()
    {
        yield return new WaitForEndOfFrame();
        CheckPoint.InitCheckPoints();
        LevelLock.InitLocks();

        bool locked = false;

        foreach (LevelLock _lock in LevelLock.Locks) {
            if (_lock.Id + 1 == MetaSceneDate.GameData.CurrentLocation.Level) {
                locked = true;
                CheckPoint.UpdateCompliteLevels(MetaSceneDate.GameData.CurrentLocation.Level);
                gameObject.SetActive(false);
                break;
            }

        }

        if (locked)
            yield break;

        if (CheckPoint.PointsCount == MetaSceneDate.GameData.CurrentLocation.Level - 1) {
            CheckPoint.UpdateCompliteLevels(MetaSceneDate.GameData.CurrentLocation.Level);
            EndLocation();
            yield break;
        }

        MetaSceneDate.LevelData = CheckPoint.GetPointByLevel(MetaSceneDate.GameData.CurrentLocation.Level).Data;
        CheckPoint.UpdateCompliteLevels(MetaSceneDate.GameData.CurrentLocation.Level);

        if (MetaSceneDate.GameData.CurrentLocation.Moving)
            StartCoroutine(Moving(MetaSceneDate.GameData.CurrentLocation.Level));
    }

    private IEnumerator Ready()
    {
        yield return new WaitForSeconds(1f);
        ready = true;
    }


    private IEnumerator Moving(int level)
    {
        moving = true;

        CheckPoint checkPoint = CheckPoint.GetPointByLevel(level);
        Vector3 target = checkPoint.Position;

        spriteRenderer.color = Color.yellow;

        Vector3 newPos = Vector3.MoveTowards(transform.position, target, Time.deltaTime);
        while (newPos != target) {
            transform.position = newPos;
            yield return new WaitForEndOfFrame();
            newPos = Vector3.MoveTowards(transform.position, target, Time.deltaTime);
        }

        moving = false;
        EndMoving();
    }

    protected override void OnClick()
    {
        if (!moving && ready) {
            MetaSceneDate.LevelData = CheckPoint.GetPointByLevel(MetaSceneDate.GameData.CurrentLocation.Level).Data;
            MetaSceneDate.Level_id = MetaSceneDate.GameData.CurrentLocation.Level;
            MetaSceneDate.OptionalLevel = false;
            OldLevelButton.Selected = null;
            StartLevelMenu.self.Open();
            click?.Invoke();
        }
    }
    
    public void EndMoving()
    {
        spriteRenderer.color = Color.green;
        MetaSceneDate.GameData.CurrentLocation.Moving = false;
    }

    public void SaveCamPosition()
    {
        MetaSceneDate.GameData.CurrentLocation.CamPos = camera.position;
    }


    public static void StartLevel()
    {
        MetaSceneDate.GameData.InterstitialStep++;
        MetaSceneDate.Started = true;
        MetaSceneDate.GameData.CurrentLocation.PointPos = Self.transform.position;
        MetaSceneDate.GameData.CurrentLocation.CamPos = Self.camera.position;
        MetaSceneDate.SaveData();
        MetaSceneDate.InShop = false;
        SceneManager.LoadScene(MetaSceneDate.buttle_level_name);
    }

    public void SavePosition()
    {
        MetaSceneDate.GameData.CurrentLocation.PointPos = transform.position;
    }

    private void EndLocation()
    {
        goToNextLocation.Open();
        gameObject.SetActive(false);
    }

    public event Action click;
}
