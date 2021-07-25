using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownBarrier : MonoBehaviour
{
    public static float Value { get; private set; }

    private void Start()
    {
        Value = transform.position.y;
    }
}
