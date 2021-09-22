using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyShower : MonoBehaviour
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
        MetaSceneDate.Player.OnMoneyChange += Change;
        Change(MetaSceneDate.Player.Money);
    }

    public void Change(int value)
    {
        text.text = value.ToString();
    }

    private void OnDestroy()
    {
        MetaSceneDate.Player.OnMoneyChange -= Change;
    }
}
