using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Chest : Popit
{
    public static Chest CurrentChest { get; set; }

    [SerializeField]
    private ChestLoot loot;

    [SerializeField]
    private bool gold;

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private PopButton exitButton;
    [SerializeField]
    private Animation exitAnim;

    [SerializeField]
    private Transform itemSpawnTarget;

    private List<GameObject> dropItems;
    private int itemIndex;

    private void Start()
    {
        if (MetaSceneDate.GoldChest != gold) {
            Destroy(gameObject);
            return;
        }

        CurrentChest = this;
        Interactable.CurrentMode = Interactable.Mode.game;

        Pimple.Camera = Camera.main;

        anim.SetBool("Gold", gold);

        dropItems = loot.Unpack();
    }

    protected override void OnFull()
    {
        UpdatePimples();
        anim.SetTrigger("Open");
    }

    public void Open()
    {
        anim.speed = 0;
        NextItem();
    }


    public void NextItem()
    {
        if (itemIndex < dropItems.Count) {
            Instantiate(dropItems[itemIndex++], itemSpawnTarget.position, Quaternion.identity);
        }
        else {
            exitAnim.Play();
            exitButton.OnClick.AddListener(() => {
                SceneManager.LoadScene(MetaSceneDate.GameData.CurrentLocation.Name);
            });
        }
    }

}
