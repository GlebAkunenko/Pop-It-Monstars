using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OptionsData
{
    public float MusicVolume { get; set; } = 1;
    public bool Mute { get; set; }
    public bool Reviewed { get; set; }
}
