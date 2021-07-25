using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChengeLocation : MonoBehaviour
{
    [SerializeField]
    private string locationName;

    [Header("Для перехода на следующий уровень")]
    [SerializeField]
    private SpriteRenderer gate;
    [SerializeField]
    private Sprite openGate;

    [SerializeField]
    private GameObject arrow;

    public void Open()
    {
        gate.sprite = openGate;
        arrow.SetActive(true);
    }

    public void Change()
    {
        CurrentPoint.Self.SaveCamPosition();
        MetaSceneDate.GameData.LocationName = locationName;
        SceneManager.LoadScene(locationName);
    }
}
