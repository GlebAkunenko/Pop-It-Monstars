using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarPimple : Pimple
{
    [SerializeField]
    private SpriteRenderer star;

    [SerializeField]
    private Color enableStarColor;

    protected override void OnClick()
    {
        Dimple = true;
        star.color = enableStarColor;
    }
}
