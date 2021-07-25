using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Analytics;

public class DropItem : MonoBehaviour
{
    private Animator anim;

    private int state = 0;

    [SerializeField]
    private Type type;

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("hp", type == Type.healthPoint);
    }

    public void EndSpawning()
    {
        state++;
    }

    private void Update()
    {
        if (Input.touchCount > 0 && state == 1) {
            state++;
            anim.SetTrigger("Exit");
        }
    }

    public void Exit()
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
        other
    }


}
