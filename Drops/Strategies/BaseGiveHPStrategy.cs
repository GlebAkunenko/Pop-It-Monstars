using UnityEngine;

[CreateAssetMenu(fileName = "Base give hp stategy 1", menuName = "Base give hp stategy", order = 51)]
public class BaseGiveHPStrategy : AbstractGiveHpStrategy
{
    [SerializeField]
    private int minHealthPoints;
    [SerializeField]
    private int maxHealthPoints;

    public override int GetHealthPointsCount()
    {
        return Random.Range(minHealthPoints, maxHealthPoints + 1);
    }
}
