using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanDisabledPopButton : PopButton
{
    [SerializeField]
    private Color enabledColor;

    [SerializeField]
    private Color disabledColor;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private bool enable = true;
    public bool Enable
    {
        get { return enable; }
        set
        {
            enable = value;
            if (enable)
                spriteRenderer.color = enabledColor;
            else spriteRenderer.color = disabledColor;
        }
    }

    protected override void OnFull()
    {
        UpdatePimples();
        if (Enable)
            OnClick.Invoke(); 
    }

}
