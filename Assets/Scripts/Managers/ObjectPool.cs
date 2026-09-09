using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    

    [SerializeField] GameObject prefab;
    [SerializeField] int amount;
    [SerializeField] bool willGrow;
    public List<GameObject> pooledObjects;

    private void Start()
    {
        pooledObjects = new List<GameObject>();

        for (int i = 0; i< amount; i++)
        {
            growPool();
        }
    }

    private GameObject growPool()
    {
        GameObject obj = Instantiate(prefab, transform);

        pooledObjects.Add(obj);

        obj.SetActive(false);
        return obj;
    }

    public GameObject GetPooledObject()
    {
        foreach (GameObject obj in pooledObjects)
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }

        if (willGrow)
        {
            return growPool();
        }

        Debug.LogError("Pool has no avaliable room");
        return null;
    }

    public void ResetGame()
    {
        foreach (GameObject obj in pooledObjects)
        {
            obj.SetActive(false);
            
        }
    }
}
