using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    [SerializeField]
    private Sprite picture;

    protected Sprite Picture { get => picture; set => picture = value; }

    public int Count { get; set; }
}
