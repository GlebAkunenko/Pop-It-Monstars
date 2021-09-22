using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMonster : Monster
{
    [SerializeField]
    private ParticalMonster unity;

    [SerializeField]
    private Vector3 buttlePosition;
    [SerializeField]
    private Vector3 buttleScale;

    protected override void OnFull()
    {
        unity.Health--;
        ValueShower.Monster.ReduceValue();
        anim.SetTrigger("die");
        audio.PlayOneShot(deathSound);
    }

    protected override void OnDead()
    {
        unity.ReduseFallMonsers();
        unity.AddDeadMonster();
        Destroy(gameObject);
    }

}
