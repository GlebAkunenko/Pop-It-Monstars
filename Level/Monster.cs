using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Monster : Popit
{
    public int Health { get; set; }

    private Animator anim;
    private new AudioSource audio;

    [SerializeField]
    private AudioClip[] hitSounds;
    [SerializeField]
    private AudioClip deathSound;

    [SerializeField]
    private SpriteRenderer modelRenderer;


    private void Start()
    {
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }


    public void Kill()
    {
        Health = 1;
        OnFull();
    }

    protected override void OnFull()
    {
        Health--;
        ValueShower.Monster.ReduceValue();
        if (Health == 0) {
            anim.SetTrigger("die");
            audio.PlayOneShot(deathSound);
            LevelController.Self.TimePause = true;
        }
        else UpdatePimples();

        //anim.SetFloat("difficultcy", MetaSceneDate.Difficulty);
    }

    public void OnSpawn()
    {
        LevelController.Self.TimePause = false;
    }

    private void OnDead()
    {
        LevelController.Self.NextMonster();
        Destroy(transform.parent.gameObject);
    }


    public void Damage()
    {
        PlayerInfo.Self.GetDamage();
        anim.SetTrigger("hit");
        audio.PlayOneShot(hitSounds[Random.Range(0, hitSounds.Length)]);
    }

    public void Damage(AudioClip sound)
    {
        PlayerInfo.Self.GetDamage();
        anim.SetTrigger("hit");
        audio.PlayOneShot(sound);
    }
}
