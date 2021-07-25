using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item collection", menuName = "Item collection", order = 51)]
public class ItemCollection : ScriptableObject
{
    [SerializeField]
    private GameObject[] itemPrefabs;

    public GameObject RandomItem()
    {
        return itemPrefabs[Random.Range(0, itemPrefabs.Length)];
    }
}
