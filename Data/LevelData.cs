using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    [SerializeField]
    private MonsterData[] monsters;
    [SerializeField]
    private Sprite background;
    [SerializeField]
    private int experience;
    [SerializeField]
    private bool timeLevel;
    [SerializeField]
    private float timeCoefficient = 1;
    [SerializeField]
    private bool autoColored;

    public int Experience { get => experience; set => experience = value; }
    public Sprite Background { get => background; set => background = value; }
    public MonsterData[] Monsters { get => monsters; set => monsters = value; }
    public bool TimeLevel { get => timeLevel; set => timeLevel = value; }

    public int ThreeStars
    {
        get
        {
            float res = 0;
            foreach (MonsterData md in Monsters)
                res += md.Time * md.Health;
            return Mathf.CeilToInt(res * 10 * timeCoefficient);
        }
    }

    public int TwoStars
    {
        get
        {
            float res = 0;
            foreach (MonsterData md in Monsters)
                res += md.Time * md.Health;
            return Mathf.CeilToInt(res * 1.5f * timeCoefficient) * 10;
        }
    }

    public bool AutoColored { get => autoColored; set => autoColored = value; }
}

[System.Serializable]
public class MonsterData
{
    [SerializeField]
    private MonsterInfo monster;
    [SerializeField]
    private GameObject orMonsterPrefab;
    [Range(2, 4)]
    [SerializeField]
    private int health = 3;
    [SerializeField]
    private Color color;
    [SerializeField]
    private bool autoColor;

    public GameObject Monster 
    {
        get
        {
            if (monster != null)
                return monster.MonsterPrefab;
            return orMonsterPrefab;
        }
    }
    public int Health { get => health; set => health = value; }
    public Color Color { get => color; set => color = value; }
    public float Time { get => monster != null ? monster.TimeForOneDamage : 0; }
    public bool AutoColor { get => autoColor; set => autoColor = value; }
}