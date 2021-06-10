using UnityEngine;

public interface Spawnable
{
    public abstract void SpawnItem();
    public abstract void DeSpawnItem();
    public abstract int GetPositionX();
    public abstract int GetPositionY();
    public abstract Transform GetPosition();
}
