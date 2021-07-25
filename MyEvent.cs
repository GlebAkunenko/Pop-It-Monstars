using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MyEvent : MonoBehaviour
{
    [SerializeField]
    private UnityEvent[] events;

    public void CallEvent(int id)
    {
        events[id].Invoke();
    }
}
