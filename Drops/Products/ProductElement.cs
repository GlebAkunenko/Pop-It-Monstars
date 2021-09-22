using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProductElement : MonoBehaviour
{
    [SerializeField]
    private Product product;

    [SerializeField]
    private TextMeshPro costText;

    private void Start()
    {
        costText.text = product.Cost.ToString();
    }

    public void Click()
    {
        BuyWindow.self.Open(product);
    }
}
