using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BuyWindow : Window
{
    public static BuyWindow self { get; private set; }

    private Animator anim;

    [SerializeField]
    private PopButton buyButton;

    [SerializeField]
    private Transform spawnPreviewProductsTarget;

    [SerializeField]
    private TextMeshProUGUI coinsText;

    private GameObject previewProduct;
    private new Transform transform;

    private void Start()
    {
        self = this;
        anim = GetComponent<Animator>();
        transform = GetComponent<Transform>();

        coinsText.text = MetaSceneDate.Player.Money.ToString();
    }

    public void Open(Product product)
    {
        buyButton.OnClick.RemoveAllListeners();

        previewProduct = Instantiate(product.DisplayPrefab, spawnPreviewProductsTarget.position, Quaternion.identity, spawnPreviewProductsTarget);
        previewProduct.transform.localScale = new Vector3(1, 1, 1);

        if (MetaSceneDate.Player.Money < product.Cost)
            buyButton.gameObject.SetActive(false);
        else {
            buyButton.gameObject.SetActive(true);
            buyButton.OnClick.AddListener( () => {
                MetaSceneDate.Player.Money -= product.Cost;
                coinsText.text = MetaSceneDate.Player.Money.ToString();
                product.GiveProduct();
            });
        }

        Interactable.CurrentMode = Interactable.Mode.canvas;
        anim.SetBool("opened", true);
    }

    public void Close()
    {
        anim.SetBool("opened", false);
    }

    public override void OnClose()
    {
        Interactable.CurrentMode = Interactable.Mode.game;
        Destroy(previewProduct);
    }

    public override void OnOpen()
    {

    }

    public void ExitFromShop()
    {
        MetaSceneDate.SaveData();
        SceneManager.LoadScene(MetaSceneDate.GameData.LocationName);
    }
}
