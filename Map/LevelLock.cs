using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelLock : MonoBehaviour
{
    public static List<LevelLock> Locks { get; set; } = new List<LevelLock>();

    [SerializeField]
    private int count;

    [SerializeField]
    private TextMesh countText;

    [SerializeField]
    private CheckPoint point;

    [SerializeField]
    private Transform reviewButtonTarget;
    
    public int Id { private set; get; }
    public int Count { get => count; set => count = value; }

    private void Start()
    {
        Locks.Add(this);
        countText.text = Count.ToString();
    }

    public void Remove()
    {
        Destroy(gameObject);
    }


    public static void InitLocks()
    {
        List<LevelLock> brush = new List<LevelLock>();

        foreach (LevelLock L in Locks) {
            if (L.Count <= MetaSceneDate.GameData.CurrentLocation.AllStars)
                brush.Add(L);
            else
                L.Id = L.point.Id;
        }

        if (brush.Count > 0) {
            int m = 0;
            Vector3 reviewTarget = new Vector3();

            foreach (LevelLock L in brush) {
                if (L.Count > m) {
                    m = L.Count;
                    reviewTarget = L.reviewButtonTarget.position;
                }
                L.Remove();
            }

            if (!MetaSceneDate.OptionsData.Reviewed)
                ReviewButton.self.SpawnHere(reviewTarget);
        }
    }

    private void OnDestroy()
    {
        Locks.Remove(this);
    }
}
