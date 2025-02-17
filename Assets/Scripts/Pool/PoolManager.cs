
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

    // Debugging
    private int poolCount1;
    private int poolCount2;
    private int poolCount3;
    private int poolCount4;

    private void Awake()
    {
        _nameToPool = new Dictionary<string, Pool>();
        
        foreach (var pool in poolData)
        {
            var tempPool = new Pool(pool, poolParent);
            _nameToPool[pool.poolName] = tempPool;
        }
    }

    private void Update()
    {
        poolCount1 = _nameToPool["BubbleManager"].Count();
        poolCount2 = _nameToPool["JailBubblePool"].Count();
        poolCount3 = _nameToPool["DeathBubblePool"].Count();
        poolCount4 = _nameToPool["YarnBubblePool"].Count();
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
