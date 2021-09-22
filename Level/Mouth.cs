using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mouth : Interactable
{
    [SerializeField]
    private Monster popit;

    [SerializeField]
    private bool animated = true;

    [SerializeField]
    private float time;
    [SerializeField]
    private Sprite[] sprites;

    [SerializeField]
    private AudioClip[] speñialSound = new AudioClip[0];

    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private new Collider2D collider;

    private bool canHit = true;

    private new void Start()
    {
        base.Start();
        
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    private new void Update()
    {
        // override the interact.Update() with these touch system

        if (CurrentMode == Mode.game) {
            if (canHit) {
                foreach (Touch touch in Input.touches) {
                    Vector3 pos = LevelController.Self.GetComponent<Camera>().ScreenToWorldPoint(touch.position);
                    Vector2 proection = new Vector3(pos.x, pos.y, transform.position.z);
                    if (collider.ClosestPoint(proection) == proection && collider.enabled) {
                        OnClick();
                        break;
                    }
                }
            }
        }
    }

    protected override void OnClick()
    {
        if (popit.Health == 0 && popit as MiniMonster == null)
            return;

        canHit = false;

        if (speñialSound.Length == 0)
            popit.Damage();
        else popit.Damage(speñialSound[Random.Range(0, speñialSound.Length)]);

        if (animated)
            StartCoroutine(Animation());
        else StartCoroutine(SwitchHitAfterSecond());
        
    }

    private IEnumerator SwitchHitAfterSecond()
    {
        yield return new WaitForSeconds(1);
        canHit = true;
    }

    private IEnumerator Animation()
    {
        if (sprites.Length == 0)
            Debug.LogError("no sprited in array. By Gleb1000");

        foreach (Sprite sprite in sprites)
        {
            spriteRenderer.sprite = sprite;
            yield return new WaitForSeconds(time);
        }

        canHit = true;
    }


}
