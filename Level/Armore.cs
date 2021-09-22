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
        DisableColliders();
    }

    protected void DisableColliders()
    {
        foreach (Collider2D coll in protectedColliders)
            coll.enabled = false;
    }

    protected override void OnFull()
    {
        EnableColliders();
        anim.Play();
    }

    protected void EnableColliders()
    {
        foreach (Collider2D coll in protectedColliders)
            coll.enabled = true;
    }

    public void Remove()
    {
        Destroy(gameObject);
    }


}
