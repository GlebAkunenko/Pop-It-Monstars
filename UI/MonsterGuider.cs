using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGuider : Guider
{
    public Animation[] monstersParts;

    private void Start()
    {
        if (MetaSceneDate.IsGuideShown(Guide.monster))
            Destroy(gameObject);
        else gameObject.SetActive(false);
    }

    public override void Init()
    {
        guideName = Guide.monster;
        showOnStart = true;
        gameObject.SetActive(true);
        base.Init();
    }

    public override void StartGuide()
    {
        Interactable.CurrentMode = Interactable.Mode.none;
        animator.speed = 1;
        foreach (Animation anim in monstersParts)
            anim.Stop();
    }

    protected override void EndGuide()
    {
        Interactable.CurrentMode = Interactable.Mode.game;
        foreach (Animation anim in monstersParts)
            anim.Play();
        base.EndGuide();
    }
}
