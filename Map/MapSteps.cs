using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSteps : MonoBehaviour
{
    private new Transform transform;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        transform = GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        spriteRenderer.size = new Vector2(spriteRenderer.size.x, CurrentPoint.PosY);
    }
}
