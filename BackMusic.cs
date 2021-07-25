using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackMusic : MonoBehaviour
{
    public static BackMusic self { get; set; }
    public AudioSource Audio { get; set; }

    public void Init()
    {
        Audio = GetComponent<AudioSource>();
        Audio.volume = MetaSceneDate.OptionsData.MusicVolume;
        Audio.mute = MetaSceneDate.OptionsData.Mute;

        DontDestroyOnLoad(gameObject);
        self = this;
    }

    public void ChangeVolume(Slider slider)
    {
        Audio.volume = slider.value;
    }
}
