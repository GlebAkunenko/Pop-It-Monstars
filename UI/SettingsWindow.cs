using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsWindow : Window
{
    [SerializeField]
    private Slider volumeSlider;
    [SerializeField]
    private GameObject muteCross;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();

        if (BackMusic.self == null)
            volumeSlider.gameObject.SetActive(false);

        muteCross.SetActive(BackMusic.self.Audio.mute);
    }

    public void ChangeVolume(Slider slider)
    {
        BackMusic.self.Audio.volume = slider.value;
        MetaSceneDate.OptionsData.MusicVolume = slider.value;
    }

    public void Open()
    {
        anim.SetBool("opened", true);
        Interactable.CurrentMode = Interactable.Mode.canvas;
    }

    public void MuteButton()
    {
        BackMusic.self.Audio.mute = !BackMusic.self.Audio.mute;
        muteCross.SetActive(BackMusic.self.Audio.mute);
        MetaSceneDate.OptionsData.Mute = BackMusic.self.Audio.mute;
    }

    public void Close()
    {
        anim.SetBool("opened", false);
    }

    public override void OnClose()
    {
        Interactable.CurrentMode = Interactable.Mode.game;
    }

    public override void OnOpen()
    {

    }
}
