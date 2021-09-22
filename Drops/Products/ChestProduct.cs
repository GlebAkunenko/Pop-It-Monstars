using UnityEngine.SceneManagement;
using UnityEngine;

public class ChestProduct : Product
{
    [SerializeField]
    private ChestLoot chestLoot;

    public override void GiveProduct()
    {
        MetaSceneDate.ChestLoot = chestLoot;
        MetaSceneDate.InShop = true;
        SceneManager.LoadScene(MetaSceneDate.chest_scene_name);
    }
}
