using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerStats
{
    public int Money { get; set; }
    public int AmountDamage { get; set; }

    private int experience;
    public int Experience
    {
        get { return experience; }
        set
        {
            experience = value;
            while (experience >= MaxExperience) {
                experience -= MaxExperience;
                Level++;
            }
            OnExperienceChange?.Invoke(Experience);
        }
    }

    private int level;
    public int Level
    {
        get { return level; }
        set
        {
            level = value;
            OnLevelChange?.Invoke(Level);
        }
    }

    private int healthPoints;
    public int HealthPoints
    {
        get { return healthPoints; }
        set
        {
            healthPoints = value;
            OnHealthPointsChange?.Invoke(healthPoints);
        }
    }

    public static PlayerStats Empty
    {
        get
        {
            PlayerStats ret = new PlayerStats(
                level: 1,
                experience: 0,
                money: 0,
                healthPoints: 8
                );
            return ret;
        }
    }

    public PlayerStats(int level, int experience, int money, int healthPoints)
    {
        this.level = level;
        this.experience = experience;
        this.Money = money;
        this.healthPoints = healthPoints;
        this.AmountDamage = 0;
    }

    [field: NonSerialized]
    public event Action<int> OnLevelChange;
    [field: NonSerialized]
    public event Action<int> OnExperienceChange;
    [field: NonSerialized]
    public event Action<int> OnHealthPointsChange;


    public int MaxExperience
    {
        get { return Level * 50; }
    }

}