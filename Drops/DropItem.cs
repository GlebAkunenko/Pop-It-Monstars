using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Analytics;

public class DropItem : MonoBehaviour
{
    private Animator anim;

    protected int state = 0;

    [SerializeField]
    protected Type type;

    protected void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("hp", type == Type.healthPoint);
    }

    public void EndSpawning()
    {
        state++;
    }

    protected void Update()
    {
        if (Input.touchCount > 0 && state == 1) {
            state++;
            anim.SetTrigger("Exit");
        }
    }

    public virtual void Exit()
    {
        if (type == Type.healthPoint) {
            MetaSceneDate.Player.HealthPoints++;
        }

        Chest.CurrentChest.NextItem();
        Destroy(gameObject);
    }

    public enum Type
    {
        healthPoint,
        money,
        other
    }


}
