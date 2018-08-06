using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

    private Queue<PooledObject> availableObjects = new Queue<PooledObject>();

    public PooledObject GetObject()
    {
        if (availableObjects.Count == 0)
            return null;
        PooledObject returnedObject = availableObjects.Dequeue();
        returnedObject.gameObject.SetActive(true);
        return returnedObject;
    }

    public void AddObject(PooledObject newObject)
    {
        availableObjects.Enqueue(newObject);
        newObject.transform.parent = this.transform;
        newObject.gameObject.SetActive(false);
    }

}
