using UnityEngine;

public class PooledObject : MonoBehaviour {

    private ObjectPool pool;
    public ObjectPool Pool
    {
        set
        {
            pool = value;
        }
    }

    public void ReturnToPool()
    {
        if (pool == null)
        {
            Debug.LogError("No Object Pool set for " + this.name);
        }

        pool.AddObject(this);
    }
}
