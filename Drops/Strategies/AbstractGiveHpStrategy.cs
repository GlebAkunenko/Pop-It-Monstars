using UnityEngine;

public abstract class AbstractGiveHpStrategy : ScriptableObject
{
    public abstract int GetHealthPointsCount();
}
