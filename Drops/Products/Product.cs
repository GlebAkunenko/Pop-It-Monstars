using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Product : MonoBehaviour
{
    [SerializeField]
    private int cost;

    [SerializeField]
    private GameObject displayPrefab;

    public int Cost { get => cost; set => cost = value; }
    public GameObject DisplayPrefab { get => displayPrefab; set => displayPrefab = value; }

    public abstract void GiveProduct();

}
