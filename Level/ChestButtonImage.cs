using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestButtonImage : MonoBehaviour
{
    [SerializeField]
    private string location;
    [SerializeField]
    private ChestLoot loot;

    public string Location { get => location; set => location = value; }
    public ChestLoot Loot { get => loot; set => loot = value; }
}
