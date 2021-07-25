using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo Self { private set; get; }

    public int Health { get; private set; }

    private void Start()
    {
        Self = this;

        if (MetaSceneDate.Player.HealthPoints > 0)
            Health = Mathf.Min(ValueShower.player_hp_max_count, MetaSceneDate.Player.HealthPoints);
        else
            Health = 1;

        ValueShower.Player.UpdateValue(Health);
    }

    public void GetDamage()
    {
        ValueShower.Player.ReduceValue();
        Health--;
        MetaSceneDate.Statistics.HpDelta--;
        MetaSceneDate.Statistics.LoseHP++;

        if (MetaSceneDate.Player.HealthPoints > 0)
            MetaSceneDate.Player.HealthPoints--;

        if (Health == 0) 
            LevelController.Self.Lose();
    }

    public void AddLife()
    {
        ValueShower.Player.UpdateValue(1);
        Health = 1;
    }


}
