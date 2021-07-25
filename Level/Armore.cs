using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armore : Popit
{
    [SerializeField]
    private Collider2D[] protectedColliders;

    private Animation anim;

    private void Start()
    {
        anim = GetComponent<Animation>();
        foreach (Collider2D coll in protectedColliders)
            coll.enabled = false;
    }

    protected override void OnFull()
    {
        foreach (Collider2D coll in protectedColliders)
            coll.enabled = true;
        anim.Play();
    }

    public void Remove()
    {
        Destroy(gameObject);
    }


}
