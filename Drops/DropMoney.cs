using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropMoney : DropItem
{
    [SerializeField]
    private new ParticleSystem particleSystem;
    

    [SerializeField]
    private TextMeshPro text;

    private Chest chest;

    private int count;

    private new void Start()
    {
        
    }

    public void Init(int count, Chest sender)
    {
        type = Type.money;
        base.Start();
        this.count = count;
        chest = sender;
        text.text = count.ToString();
    }

    private new void Update()
    {
        if (Input.touchCount > 0 && state == 1) {
            state++;
            particleSystem.Play();
            Destroy(gameObject, 2);
            StartCoroutine(AddMoney());
        }
    }

    private IEnumerator AddMoney()
    {
        yield return new WaitForSeconds(0.9f);
        GetComponent<SpriteRenderer>().enabled = false;
        Destroy(text.gameObject);
        MetaSceneDate.Player.Money += count;
        chest.NextItem();
    }
}
