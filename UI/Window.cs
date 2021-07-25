using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Window : MonoBehaviour
{
    public abstract void OnOpen();

    public abstract void OnClose();
}
