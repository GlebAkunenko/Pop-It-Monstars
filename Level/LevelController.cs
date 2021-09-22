using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Analytics;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public static LevelController Self { private set; get; }

    [SerializeField]
    private string menuSceneName = "Menu";
    public string MenuSceneName { get => menuSceneName; set => menuSceneName = value; }

    [SerializeField]
    private Transform spawnTarget;
    [SerializeField]
    private Transform leftTarget;
    [SerializeField]
    private Transform rightTarget;
    [SerializeField]
    private Transform middleTarget;

    private new Camera camera;
    public Camera Camera { get => camera; set => camera = value; }

    public bool TimePause { get; set; } = true;
    public byte Stars { private set; get; }
    public bool Win { get; set; }

    private bool notLose;
    public bool NotLose
    {
        get { return notLose && !MetaSceneDate.LevelData.TimeLevel; }
    }

    public bool OpenVictoryWindow { private set; get; }
    public AudioSource SceneSounds { get; set; }

    [SerializeField]
    private bool enableMounseDown;

    private int nextMonster = 0;
    private int time;
    private int last_time = 0;
    private float time_limit = 0;
    private Interactable.Mode lastMode;
    private Animator currentMonterAnim;

    public bool GameGoing
    {
        get { return currentMonterAnim != null; }
    }

    [SerializeField]
    private Button skipButton;
    [SerializeField]
    private VictoryWindow victoryWindow;
    [SerializeField]
    private LoseWindow loseWindow;
    [SerializeField]
    private GameObject timer;
    [SerializeField]
    private GameObject starsFrame;
    [SerializeField]
    private GameObject pausePanel;

    [SerializeField]
    private Image[] progressStars = new Image[3];
    private bool[] pulsedStar = new bool[3];

    private Coroutine counter;
    private TextMeshProUGUI timeText;


    private bool analiticsWriteEnd;

    private bool gamePause;
    public bool GamePause
    {
        get { return gamePause; }
        set
        {
            if (gamePause == value)
                return;

            gamePause = value;
            if (gamePause) {
                pausePanel.SetActive(true);
                lastMode = Interactable.CurrentMode;
                currentMonterAnim.speed = 0;
                Interactable.CurrentMode = Interactable.Mode.pause;
            }
            else {
                pausePanel.SetActive(false);
                currentMonterAnim.speed = 1;
                Interactable.CurrentMode = lastMode;
            }
        }
    }

    public Transform LeftTarget { get => leftTarget; set => leftTarget = value; }
    public Transform RightTarget { get => rightTarget; set => rightTarget = value; }
    public Transform MiddleTarget { get => middleTarget; set => middleTarget = value; }

    private void Start()
    {
        Self = this;

        Camera = GetComponent<Camera>();
        SceneSounds = GetComponent<AudioSource>();

        foreach (Image star in progressStars)
            star.fillAmount = 0;

        Interactable.CurrentMode = Interactable.Mode.game;
        Monster.UsedColors.Clear();

        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart,
            new Parameter(FirebaseAnalytics.ParameterLevelName, MetaSceneDate.AnaliticsLevelName));

        MetaSceneDate.Statistics = new LevelStatistics();

        timeText = timer.GetComponent<TextMeshProUGUI>();
        if (MetaSceneDate.LevelData.TimeLevel)
            starsFrame.SetActive(false);
        else timer.SetActive(false);

    }

    public void CheckForWin()
    {
        if (Win && PlayerInfo.Self.Health > 0)
            StartCoroutine(Victory());
    }

    public void StartGame()
    {
        if (MetaSceneDate.LevelData.TimeLevel)
            counter = StartCoroutine(Counter());
        NextMonster();
    }

    public void Exit()
    {
        if (!OpenVictoryWindow && !Win) {
            if (NotLose) StartCoroutine(Victory());
            else SceneManager.LoadScene(MetaSceneDate.GameData.LocationName);
        }
    }


    public void NextMonster()
    {
        if (nextMonster == MetaSceneDate.LevelData.Monsters.Length) {
            Win = true;
            UpdateStarBar();
            if (PlayerInfo.Self.Health > 0)
                StartCoroutine(Victory());
            return;
        }

        if (!MetaSceneDate.LevelData.TimeLevel)
            UpdateStarBar();

        MonsterData monsterData = MetaSceneDate.LevelData.Monsters[nextMonster++];

        time_limit = monsterData.Time;

        Interactable.CurrentMode = Interactable.Mode.none;

        GameObject o = Instantiate(monsterData.Monster, spawnTarget.position, Quaternion.identity);
        Monster monster = o.GetComponentInChildren<Monster>();

        Color monsterColor = monsterData.Color;
        if (monsterData.AutoColor || MetaSceneDate.LevelData.AutoColored)
            monsterColor = monster.GetAutoColor(MetaSceneDate.Level_id - 1);

        ParticalMonster particalMonster = monster as ParticalMonster;
        if (particalMonster == null) {
            monster.Health = monsterData.Health;
            monster.Paint(monsterColor);
            ValueShower.Monster.ChangeFrame(monsterData.Health);
        }
        else {
            particalMonster.Paint(monsterColor);
            particalMonster.Health = particalMonster.MiniMonsters.Length;
            ValueShower.Monster.ChangeFrame(particalMonster.MiniMonsters.Length);
        }

        Monster.UsedColors.Add(monsterColor);
        
        monster.transform.localScale = Vector3.zero;
        currentMonterAnim = monster.GetComponent<Animator>();


        skipButton.onClick.RemoveAllListeners();
        skipButton.onClick.AddListener(() => { monster.Kill(); });
    }

    private void UpdateStarBar()
    {
        float new_progress = (float)nextMonster / MetaSceneDate.LevelData.Monsters.Length;
        float old_progress = Mathf.Max((float)(nextMonster - 1f) / MetaSceneDate.LevelData.Monsters.Length, 0);
        new_progress *= 3;
        old_progress *= 3;
        StartCoroutine(StarFilling(old_progress, new_progress));
        //for (int i = 0; i < 3; i++) {
        //    if (i + 1 <= progress) {
        //        progressStars[i].fillAmount = 1;
        //        if (!pulsedStar[i])
        //            progressStars[i].gameObject.GetComponent<Animation>().Play();
        //        pulsedStar[i] = true;

        //        notLose = true;
        //    }

        //}
        //if (progress < 3)
        //    StartCoroutine(StarFilling(Mathf.FloorToInt(progress), progress - Mathf.FloorToInt(progress)));
    }

    private IEnumerator StarFilling(float oldProgress, float newProgress)
    {
        float step = (newProgress - oldProgress) / 10f;
        float value = oldProgress;
        for(int i = 0; i < 10; i++) {
            value += step;
            int index = Mathf.FloorToInt(value);
            index = Mathf.Min(2, index);
            progressStars[index].fillAmount = value - index;

            if (index > 0) {
                if (!pulsedStar[index - 1]) {
                    progressStars[index - 1].fillAmount = 1;
                    progressStars[index - 1].gameObject.GetComponent<Animation>().Play();
                    pulsedStar[index - 1] = true;
                    notLose = true;
                }
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator Victory()
    {
        OpenVictoryWindow = true;

        Interactable.CurrentMode = Interactable.Mode.canvas;

        if (MetaSceneDate.LevelData.TimeLevel)
            StopCoroutine(counter);

        pausePanel.SetActive(false);


        if (MetaSceneDate.LevelData.TimeLevel) {
            if (time <= MetaSceneDate.LevelData.ThreeStars)
                Stars = 3;
            else if (time <= MetaSceneDate.LevelData.TwoStars)
                Stars = 2;
            else Stars = 1; 
        }
        else {
            if (Win) Stars = 3;
            else {
                float progress = (nextMonster - 1f) / MetaSceneDate.LevelData.Monsters.Length;
                progress *= 3;
                Stars = (byte)Mathf.FloorToInt(progress);
            }
        }

        MetaSceneDate.Statistics.Stars = Stars;

        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelEnd, new Parameter[] {
            new Parameter(FirebaseAnalytics.ParameterLevelName, MetaSceneDate.AnaliticsLevelName),
            new Parameter(FirebaseAnalytics.ParameterSuccess, 1)
        });
        analiticsWriteEnd = true;

        if (MetaSceneDate.GameData.CurrentLocation.Stars[MetaSceneDate.Level_id - 1] < Stars) {
            MetaSceneDate.GameData.CurrentLocation.Stars[MetaSceneDate.Level_id - 1] = Stars;
            MetaSceneDate.GameData.CurrentLocation.UpdateAllStars();
        }

        yield return new WaitForSeconds(1);

        victoryWindow.Open();
    }

    private IEnumerator Counter()
    {
        while(true) {

            while (TimePause || GamePause) {
                yield return new WaitForEndOfFrame();
            }

            time += 1;
            string s = (time / 10).ToString() + "." + (time % 10).ToString();
            timeText.text = s;

            yield return new WaitForSeconds(0.1f);
        }
    }


    public void Lose()
    {
        Interactable.CurrentMode = Interactable.Mode.canvas;
        TimePause = true;

        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelEnd, new Parameter[] {
            new Parameter(FirebaseAnalytics.ParameterLevelName, MetaSceneDate.AnaliticsLevelName),
            new Parameter(FirebaseAnalytics.ParameterSuccess, 0)
        });
        analiticsWriteEnd = true;

        StartCoroutine(OpenLoseWindow());
    }

    private IEnumerator OpenLoseWindow()
    {
        yield return new WaitForSeconds(1);
        loseWindow.Open();
    }

    private void OnDestroy()
    {
        if (!analiticsWriteEnd) {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelEnd, new Parameter[] {
            new Parameter(FirebaseAnalytics.ParameterLevelName, MetaSceneDate.AnaliticsLevelName),
            new Parameter(FirebaseAnalytics.ParameterSuccess, 0)
        });
        }

        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventSpendVirtualCurrency, new Parameter[] {
            new Parameter(FirebaseAnalytics.ParameterVirtualCurrencyName, "HP"),
            new Parameter(FirebaseAnalytics.ParameterValue, MetaSceneDate.Player.AmountDamage)
        });
        MetaSceneDate.Player.AmountDamage = 0;

        MetaSceneDate.SaveData();
    }
}

public enum LevelState
{
    stay,
    going
}
