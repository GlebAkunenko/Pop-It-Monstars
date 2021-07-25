using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpBarrier : MonoBehaviour
{
    public static float Value { get; private set; }

    private void Start()
    {
        Value = transform.position.y;
    }

    public void UpdateValue()
    {
        Value = transform.position.y;
    }
}
