using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelBar : MonoBehaviour
{
    public static LevelBar Self { get; private set; }

    [SerializeField]
    private TextMeshProUGUI level;
    [SerializeField]
    private Slider experience;

    private void Awake()
    {
        Self = this;
    }

    public void Init()
    {
        MetaSceneDate.Player.OnExperienceChange += ChangeExperience;
        MetaSceneDate.Player.OnLevelChange += ChangeLevel;

        ChangeExperience(MetaSceneDate.Player.Experience);
        ChangeLevel(MetaSceneDate.Player.Level);
    }

    private void ChangeExperience(int value)
    {
        experience.maxValue = MetaSceneDate.Player.MaxExperience;
        experience.value = value;
    }

    private void ChangeLevel(int value)
    {
        level.text = value.ToString();
        experience.maxValue = MetaSceneDate.Player.MaxExperience;
    }

    private void OnDestroy()
    {
        MetaSceneDate.Player.OnLevelChange -= ChangeLevel;
        MetaSceneDate.Player.OnExperienceChange -= ChangeExperience;
    }
}
