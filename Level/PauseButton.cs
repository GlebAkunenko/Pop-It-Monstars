using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : PopButton
{
    [SerializeField]
    private Sprite pause;
    [SerializeField]
    private Sprite resume;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    private Interactable.Mode pimpleMode;

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        pimpleMode = pimples[0].Type;
        OnClick.AddListener(Switch);
    }
    
    private void Switch()
    {
        if (!LevelController.Self.GameGoing)
            return;

        LevelController.Self.GamePause = !LevelController.Self.GamePause;
        if (LevelController.Self.GamePause) {
            pimples[0].Type = Interactable.Mode.pause;
            spriteRenderer.sprite = resume;
        }
        else {
            pimples[0].Type = pimpleMode;
            spriteRenderer.sprite = pause;
        }
    }
}
