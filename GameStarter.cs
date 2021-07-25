using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    [SerializeField]
    private string[] locationsName;

    [SerializeField]
    private BackMusic backMusic;
    [SerializeField]
    private Ads adsManager;

    private void Start()
    {
        MetaSceneDate.LoadData(locationsName);
        backMusic.Init();
        adsManager.Init();
    }

    public void LoadMap()
    {
        if (MetaSceneDate.GameData.CurrentLocation != null)
            SceneManager.LoadScene(MetaSceneDate.GameData.CurrentLocation.Name);
        else SceneManager.LoadScene(MetaSceneDate.first_level_scene_name);
    }
}
