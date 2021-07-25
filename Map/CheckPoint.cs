using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CheckPoint : MonoBehaviour
{
    private static List<CheckPoint> checkPoints = new List<CheckPoint>();

    public static CheckPoint GetPointByLevel(int level)
    {
        return checkPoints[level - 1];
    }

    public static int PointsCount
    {
        get { return checkPoints.Count; }
    }

    public static void InitCheckPoints()
    {
        checkPoints.Sort((x, y) => x.PosY.CompareTo(y.PosY));
        for (int i = 0; i < checkPoints.Count; i++)
            checkPoints[i].Id = i;
    }

    public int Id { get; set; }

    [SerializeField]
    private LevelData data;
    public LevelData Data
    {
        get { return data; }
    }

    [SerializeField]
    [Tooltip("Та серая кнопка")]
    private OldLevelButton compliteLevelObject;

    [SerializeField]
    private AnimationClip specialAnimation;

    private new Transform transform;

    public float PosY
    {
        get { return transform.position.y; }
    }

    public Vector3 Position
    {
        get { return transform.position; }
    }

    // this func have to start after all Start()
    public static void UpdateCompliteLevels(int currentLevel)
    {
        for (int i = 0; i < currentLevel - 1; i++) {
            string a = checkPoints[i].gameObject.name;
            checkPoints[i].compliteLevelObject.gameObject.SetActive(true);
            checkPoints[i].compliteLevelObject.SetStars(MetaSceneDate.GameData.CurrentLocation.Stars[i]);
        }
        
    }

    private void Awake()
    {
        if (checkPoints.Count > 0)
            checkPoints.Clear();
    }

    private void Start()
    {
        transform = GetComponent<Transform>();
        checkPoints.Add(this);
    }
 
}
