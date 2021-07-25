using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class HealthShower : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;

    [SerializeField]
    private bool initOnStart;

    private void Start()
    {
        if (initOnStart)
            Init();
    }

    public void Init()
    {
        MetaSceneDate.Player.OnHealthPointsChange += ChangeHP;
        ChangeHP(MetaSceneDate.Player.HealthPoints);
    }

    public void ChangeHP(int value)
    {
        text.text = value.ToString();
    }

    private void OnDestroy()
    {
        MetaSceneDate.Player.OnHealthPointsChange -= ChangeHP;
    }
}
