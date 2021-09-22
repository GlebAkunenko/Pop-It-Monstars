using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Minecart : Armore
{
    private Animator anim;

    [SerializeField]
    private Transform monster;

    [SerializeField]
    private ParticalMonster unity;

    private void Start()
    {
        anim = GetComponent<Animator>();
        DisableColliders();
    }

    protected override void OnFull()
    {
        EnableColliders();
        anim.SetTrigger("fall");
    }

    public void UnlinkMonster()
    {
        if (unity != null) {
            unity.SetMiniMonster(monster);
        }
        else {
            monster.rotation = Quaternion.identity;
            monster.position = Vector3.zero;
            monster.parent = transform.parent;
        }
    }
}
