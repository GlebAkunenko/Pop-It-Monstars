using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ValueShower : MonoBehaviour
{
    public const int player_hp_max_count = 3;

    public static ValueShower Player { private set; get; }
    public static ValueShower Monster { private set; get; }

    [SerializeField]
    private FrameBar[] frames;
    private int currentFrameId;
    
    public FrameBar CurrentFrame
    {
        get { return frames[currentFrameId]; }
    }

    [SerializeField]
    private Type valueType;

    private void Awake()
    {
        if (valueType == Type.player) {
            Player = this;
            frames[0].Update = null;
        }
        if (valueType == Type.enemy)
            Monster = this;
        
    }

    public void ReduceValue()
    {
        CurrentFrame.RemovePoint();
    }

    public void UpdateValue(int value)
    {
        CurrentFrame.UpdateToValue(value);
    }

    public void ChangeFrame(int size)
    {
        foreach (FrameBar frame in frames)
            frame.gameObject.SetActive(false);

        bool succsess = false;
        for(int i = 0; i < frames.Length; i++)
        {
            if (frames[i].Size == size)
            {
                currentFrameId = i;
                frames[i].gameObject.SetActive(true);
                frames[i].UpdateAllPoints();
                succsess = true;
                break;
            }
        }
        if (!succsess)
            Debug.LogError("Change frame error. By Gleb1000");
    }


    public enum Type
    {
        player,
        enemy
    }


}
