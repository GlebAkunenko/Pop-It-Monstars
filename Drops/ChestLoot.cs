using Firebase.Analytics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chest loot", menuName = "Chest loot", order = 51)]
public class ChestLoot : ScriptableObject
{
    [SerializeField]
    private int maxItems;

    [SerializeField]
    private AbstractGiveHpStrategy giveHPStrategy;

    [SerializeField]
    private GameObject healthPointPrefab;

    [SerializeField]
    private Loot[] loots;

    public List<GameObject> Unpack()
    {
        List<GameObject> result = new List<GameObject>();

        int hpCount = Mathf.Min(maxItems, giveHPStrategy.GetHealthPointsCount());

        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventEarnVirtualCurrency, new Parameter[] {
                new Parameter(FirebaseAnalytics.ParameterVirtualCurrencyName, "HP"),
                new Parameter(FirebaseAnalytics.ParameterValue, hpCount)
            });
        MetaSceneDate.Statistics.HpDelta += hpCount;
        MetaSceneDate.Statistics.AddHP += hpCount;

        for (int i = 0; i < hpCount; i++)
            result.Add(healthPointPrefab);
        
        foreach(Loot loot in loots) {

            bool end = false;

            foreach(int chanse in loot.CountsAndChances) {

                if (result.Count == maxItems || end)
                    break;

                int r = Random.Range(0, 101);
                if (r <= chanse)
                    result.Add(loot.Collection.RandomItem());
                else {
                    end = true;
                    break;
                }
            }
        }

        return result;
    }

    [System.Serializable]
    public class Loot
    {
        [SerializeField] [Range(0, 100)]
        private int[] countsAndChances;
        public int[] CountsAndChances { get => countsAndChances; set => countsAndChances = value; }

        [SerializeField]
        private ItemCollection collection;
        public ItemCollection Collection { get => collection; set => collection = value; }

    }

}


