
using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField]
    private List<PoolData> poolData;

    [SerializeField] 
    private Transform poolParent;

    private Dictionary<string, Pool> _nameToPool;

    private void Awake()
    {
        _nameToPool = new Dictionary<string, Pool>();
        
        foreach (var pool in poolData)
        {
            var tempPool = new Pool(pool, poolParent);
            _nameToPool[pool.poolName] = tempPool;
        }
    }

    public GameObject RetrieveFromPool(string name, Vector2 pos)
    {
        return _nameToPool[name].RetrieveFromPool(pos);
    }

    public void ReturnToPool(string name, GameObject obj)
    {
        _nameToPool[name].ReturnToPool(obj);
    }
}
