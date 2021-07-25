using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popit : MonoBehaviour
{
    private int dimpleCount = 0;
    public int DimpleCount { 
        get { return dimpleCount; }
        set
        {
            OnDimpleCountChange?.Invoke();
            dimpleCount = value;
            if (dimpleCount == pimples.Count)
                OnFull();
        }
    }

    protected List<Pimple> pimples = new List<Pimple>();

    protected event Action OnDimpleCountChange;

    [SerializeField]
    private Color color;
    public Color Color { get => color; set => color = value; }

    [SerializeField] [Tooltip("All painting objects without pimples")]
    protected SpriteRenderer[] paintObjs;



    public void AddPimple(Pimple obj)
    {
        pimples.Add(obj);
    }

    public void UpdatePimples()
    {
        foreach (Pimple p in pimples)
            p.Dimple = false;
    }

    protected virtual void OnFull()
    {
        UpdatePimples();
    }

    public virtual void Paint(Color color)
    {
        Color = color;
        foreach (SpriteRenderer sp in paintObjs)
            sp.color = color;
    }

}
