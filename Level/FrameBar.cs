using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameBar : MonoBehaviour
{
    [SerializeField]
    private GameObject[] points;

    private Animation anim;
    [SerializeField]
    private AnimationClip reduce;
    [SerializeField]
    private AnimationClip update;

    public int Size
    {
        get { return points.Length; }
    }

    public AnimationClip Reduce { get => reduce; set => reduce = value; }
    public AnimationClip Update { get => update; set => update = value; }

    private int lastPoint;

    private void Start()
    {
        anim = GetComponent<Animation>();
    }

    public void RemovePoint()
    {
        if (anim == null)
            anim = GetComponent<Animation>();

        if (lastPoint >= 0 && lastPoint < points.Length)
        {
            points[lastPoint].SetActive(false);
            lastPoint--;
            anim.clip = Reduce;
            anim.Play();
        }
        else Debug.LogError("last point out of range. By Gleb1000");
    }

    public void UpdateAllPoints()
    {
        if (anim == null)
            anim = GetComponent<Animation>();

        foreach (GameObject p in points) 
            p.SetActive(true);

        if (anim == null)
            anim = GetComponent<Animation>();

        if (Update != null) {
            anim.clip = Update;
            anim.Play();
        }

        lastPoint = points.Length - 1;

    }

    public void UpdateToValue(int value)
    {
        if (anim == null)
            anim = GetComponent<Animation>();

        if (value > points.Length)
            Debug.LogError("show value is larger than array. By Gleb1000");

        else
        {
            foreach (GameObject p in points) {
                p.SetActive(false);
            }

            for (int i = 0; i < value; i++) {
                points[i].SetActive(true);
            }
        }

        anim.clip = Update;
        anim.Play();

        lastPoint = value - 1;
    }
}
