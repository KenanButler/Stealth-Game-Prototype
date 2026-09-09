using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{

    #region Singleton
    static ObjectPoolManager instance;
    public static ObjectPoolManager Instance => instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion
    public enum PoolTypes {Bullet, Enemy}

    [SerializeField] private List<ObjectPool> pools;

    private void Start()
    {
        
        GetComponentsInChildren(pools);
    }

    public GameObject GetPooledObject(PoolTypes types)
    {
        return pools[(int)types].GetPooledObject();
    }

    public void ResetGame()
    {
        foreach (var pool in pools)
        {
            pool.ResetGame();
        }
    }



}
