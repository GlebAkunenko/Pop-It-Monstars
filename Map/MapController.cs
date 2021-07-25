using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public CurrentPoint currentPoint;

    public void OnAnimationEnd()
    {
        currentPoint.EndMoving();
    }
}
