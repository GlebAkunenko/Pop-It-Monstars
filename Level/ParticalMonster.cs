using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticalMonster : Monster
{
    [SerializeField]
    private MiniMonster[] miniMonsters;

    [SerializeField]
    private Vector3 buttleScale;

    private int falled_monsters = 0;
    private int deadMonsters = 0;
    private Transform firstMiniMonsertTransformCache;

    public override int Health 
    {
        get => base.Health;
        set
        {
            base.Health = value;
            if (base.Health < 1) {
                LevelController.Self.TimePause = true;
                OnDead();
            }
        }
    }

    public MiniMonster[] MiniMonsters { get => miniMonsters; set => miniMonsters = value; }

    public override void Paint(Color color)
    {
        base.Paint(color);
        foreach (MiniMonster miniMonster in miniMonsters)
            miniMonster.Paint(color);
    }

    public void ReduseFallMonsers()
    {
        falled_monsters--;
    }

    public void AddDeadMonster()
    {
        deadMonsters++;
        if (deadMonsters == MiniMonsters.Length)
            Destroy(gameObject);
    }

    public void SetMiniMonster(Transform monster)
    {
        // monsters position is controlled by Animation so for moving moster using trasform.parent = new...
        if (falled_monsters == 0) {
            monster.parent = LevelController.Self.MiddleTarget;
            firstMiniMonsertTransformCache = monster;
            falled_monsters = 1;
        }
        else if (falled_monsters == 1) {
            firstMiniMonsertTransformCache.parent = LevelController.Self.LeftTarget;
            monster.parent = LevelController.Self.RightTarget;
            falled_monsters = 2;
        }
        monster.rotation = Quaternion.identity;
    }

}
