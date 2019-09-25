/*-------------------------------------------------------------------*
|  Title:			ObjectPool
|
|  Author:			Thomas Maltezos
| 
|  Description:		Pool that stores gameObjects that are 'deleted'
|					often or to only allow a certain amount of
|					objects.
*-------------------------------------------------------------------*/

using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool sharedInstance;
    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;

    // Creates a shared Instance of the class.
    void Awake()
    {
        sharedInstance = this;
    }

    // Creates a list of objects with the size depending on the amountToPool input.
    void Start()
    {
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = Instantiate(objectToPool);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    // Returns the pooledObject number.
    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }
}
