using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class VictoryWindow : Window
{
    public static VictoryWindow self { private set; get; }

    [SerializeField]
    private Slider experienceSlider;

    [SerializeField]
    private Text level;      
    [SerializeField]
    private TextMeshProUGUI proLevel;        

    [SerializeField]
    private ChestButtonLevel silverChest;
    [SerializeField]
    private ChestButtonLevel goldChest;
    [SerializeField]
    private ChestButtonLevel optionalAdsSilverChest;
    [SerializeField]
    private PopButton exit;
    [SerializeField]
    private Animation optionalExit;    
    [SerializeField]
    private Animator[] stars;


    private Animator anim;

    private bool addingExp;
    private int starsCount;
    private int endExperience;

    private void Start()
    {
        self = this;

        anim = GetComponent<Animator>();
        transform.localScale = Vector3.zero;

        experienceSlider.minValue = 0;
        experienceSlider.maxValue = MetaSceneDate.Player.MaxExperience;

        ChangeLevel(MetaSceneDate.Player.Level);
        ChangeExperience(MetaSceneDate.Player.Experience);

        if (MetaSceneDate.OptionalLevel) {
            exit.gameObject.SetActive(true);
            Destroy(silverChest.gameObject);
            Destroy(goldChest.gameObject);
        }
        else {
            exit.gameObject.SetActive(false);
            optionalAdsSilverChest.gameObject.SetActive(false);
        }
    }

    public void Open()
    {
        anim.SetBool("opened", true);

        MetaSceneDate.Player.OnExperienceChange += ChangeExperience;
        MetaSceneDate.Player.OnLevelChange += ChangeLevel;

        if (!MetaSceneDate.OptionalLevel) {
            MetaSceneDate.GameData.CurrentLocation.Level++;
            MetaSceneDate.GameData.CurrentLocation.Moving = true;
        }
    }

    public void NextStar()
    {
        if (starsCount == LevelController.Self.Stars) {
            if (!addingExp) {
                StartCoroutine(AddingExperience());
                endExperience = MetaSceneDate.Player.Experience + MetaSceneDate.LevelData.Experience;

                if (!MetaSceneDate.OptionalLevel) {
                    silverChest.Spawn();
                    silverChest.CanOpen = true;
                }
                else {
                    optionalExit.Play();
                    exit.OnClick.AddListener(() => {
                        SceneManager.LoadScene(MetaSceneDate.GameData.CurrentLocation.Name);
                    });
                }
            }
            return;
        }

        stars[starsCount++].SetTrigger("Go");
    }

    public void SaveExperiance()
    {
        StopAllCoroutines();
        MetaSceneDate.Player.Experience = endExperience;
        MetaSceneDate.Win = true;
    }

    private void ChangeLevel(int value)
    {
        if (level != null)
            level.text = value.ToString();
        if (proLevel != null)
            proLevel.text = value.ToString();
    }

    private void ChangeExperience(int value)
    {
        experienceSlider.value = value;
    }

    public override void OnOpen()
    {
        if (!MetaSceneDate.OptionalLevel) {
            goldChest.CanOpen = true;
            goldChest.Spawn();
        }
        else {
            optionalAdsSilverChest.CanOpen = true;
            optionalAdsSilverChest.Spawn();
        }
        NextStar();
    }

    private IEnumerator AddingExperience()
    {
        addingExp = true;

        int buffer = MetaSceneDate.LevelData.Experience;

        float s = buffer / 40;
        int step = Mathf.Max(1, Mathf.CeilToInt(s));

        while (buffer > 0) {
            MetaSceneDate.Player.Experience += step;
            buffer -= step;
            yield return new WaitForSeconds(0.05f);
        }
        MetaSceneDate.Player.Experience += buffer;

        MetaSceneDate.Win = true;

        addingExp = false;
    }

    private void OnDestroy()
    {
        MetaSceneDate.Player.OnLevelChange -= ChangeLevel;
        MetaSceneDate.Player.OnExperienceChange -= ChangeExperience;
    }

    public override void OnClose()
    {
        throw new System.NotImplementedException();
    }
}
