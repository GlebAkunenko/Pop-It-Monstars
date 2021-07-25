using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public static Mode CurrentMode { get; set; } = Mode.game;
    public Mode Type { get => type; set => type = value; }

    protected Camera main;

    [SerializeField]
    protected Collider2D touchCollider;
    protected new Transform transform;

    [SerializeField]
    private Mode type;

    protected void Start()
    {
        main = Camera.main;
        transform = GetComponent<Transform>();
    }

    protected void Update()
    {
        if (CurrentMode == type) {
            foreach (Touch touch in Input.touches) {
                if (touch.phase == TouchPhase.Began) {
                    Vector3 pos = main.ScreenToWorldPoint(touch.position);
                    if (touchCollider.bounds.Contains(new Vector3(pos.x, pos.y, transform.position.z))) {
                        OnClick();
                        break;
                    }
                }
            }
        }
    }

    protected abstract void OnClick();

    public enum Mode
    {
        game,
        canvas,
        pause,
        none
    }
}
