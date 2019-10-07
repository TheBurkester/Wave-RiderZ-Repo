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

[System.Serializable] // Allows multiple objects to be pooled simultaniously.
public class ObjectPoolItem
{
    public GameObject objectToPool;
    public int amountToPool;
}

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool sharedInstance;
    private List<GameObject> pooledObjects;
    public List<ObjectPoolItem> itemsToPool;
   
    // Creates a shared Instance of the class.
    void Awake()
    {
        sharedInstance = this;
    }

    // Creates a list of objects with the size depending on the amountToPool input.
    void Start()
    {
        pooledObjects = new List<GameObject>();
        foreach (ObjectPoolItem item in itemsToPool)
        {
            for (int i = 0; i < item.amountToPool; i++)
            {
                GameObject obj = Instantiate(item.objectToPool);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }
    }

    // Returns the pooledObject number via tag.
    public GameObject GetPooledObject(string tag)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].tag == tag)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }
}
