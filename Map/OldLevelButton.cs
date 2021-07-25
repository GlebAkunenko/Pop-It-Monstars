using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class OldLevelButton : Interactable
{
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Color defaultColor;
    [SerializeField]
    private Color selectedColor;
    [SerializeField]
    private GameObject[] stars;

    private static OldLevelButton selected;
    public static OldLevelButton Selected
    {
        get { return selected; }
        set
        {
            if (selected != null)
                selected.spriteRenderer.color = selected.defaultColor;

            selected = value;

            if (selected != null)
                selected.spriteRenderer.color = selected.selectedColor;
        }
    }

    [SerializeField]
    private CheckPoint checkPoint;
    public CheckPoint CheckPoint { get => checkPoint; }

    public void SetStars(int count)
    {
        if (count > 3)
            Debug.LogError("over 3 stars; By Gleb1000");

        foreach (GameObject s in stars)
            s.SetActive(false);

        for (int i = 0; i < count; i++)
            stars[i].SetActive(true);
    }

    private new void Start()
    {
        base.Start();

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = defaultColor;
    }



    protected override void OnClick()
    {
        Selected = this;
        MetaSceneDate.LevelData = CheckPoint.Data;
        MetaSceneDate.OptionalLevel = true;
        MetaSceneDate.Level_id = CheckPoint.Id + 1;
        StartLevelMenu.self.Open();
    }
}
