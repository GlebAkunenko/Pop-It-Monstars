using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReviewButton : PopButton
{
    public static ReviewButton self { get; set; }
    public static Vector3 Position { get; set; }

    [SerializeField]
    private Sprite happy;
    [SerializeField]
    private Sprite sad;

    private void Awake()
    {
        self = this;
    }

    public void Init()
    {
        if (MetaSceneDate.OptionsData.Reviewed) {
            Position = MetaSceneDate.GameData.CurrentLocation.ReviewPos;
            transform.position = Position;
            GetComponent<SpriteRenderer>().sprite = happy;
        }
    }

    public void SpawnHere(Vector3 position)
    {
        gameObject.SetActive(true);
        transform.position = position;
        MetaSceneDate.GameData.CurrentLocation.ReviewPos = position;
        GetComponent<SpriteRenderer>().sprite = MetaSceneDate.OptionsData.Reviewed ? happy : sad;
    }

    public void Click()
    {
        CurrentPoint.Self.SavePosition();
        CurrentPoint.Self.SaveCamPosition();
        SceneManager.LoadScene(MetaSceneDate.review_scene_name);
    }


}
