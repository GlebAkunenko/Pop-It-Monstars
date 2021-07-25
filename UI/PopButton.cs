using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PopButton : Popit
{
    [SerializeField]
    private UnityEvent onClick;

    public UnityEvent OnClick { get => onClick; set => onClick = value; }

    protected override void OnFull()
    {
        UpdatePimples();
        OnClick.Invoke();
    }
}
