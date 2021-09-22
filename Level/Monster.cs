using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Monster : Popit
{
    public virtual int Health { get; set; }

    public static List<Color> UsedColors { get; set; } = new List<Color>();

    protected Animator anim;
    protected new AudioSource audio;

    [SerializeField]
    protected AudioClip[] hitSounds;
    [SerializeField]
    protected AudioClip deathSound;

    [SerializeField]
    private SpriteRenderer modelRenderer;

    [SerializeField]
    private MonsterGuider guider;

    [SerializeField]
    private Color[] suitableColores;


    private void Start()
    {
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    // for testing or debug
    public void Kill()
    {
        Health = 1;
        OnFull();
    }

    public Color GetAutoColor(int seed)
    {
        int n = suitableColores.Length;
        for(int i = seed; i < seed + n; i++) {
            if (!UsedColors.Contains(suitableColores[i % n]))
                return suitableColores[i % n];
        }
        return suitableColores[seed % n];
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
        if (Interactable.CurrentMode != Interactable.Mode.pause)
            Interactable.CurrentMode = Interactable.Mode.game;
        LevelController.Self.TimePause = false;
    }

    protected virtual void OnDead()
    {
        LevelController.Self.NextMonster();
        Destroy(transform.parent.gameObject);
    }

    public override void Paint(Color color)
    {
        base.Paint(color);
    }

    public void StartGuide()
    {
        if (guider != null)
            guider.Init();
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
