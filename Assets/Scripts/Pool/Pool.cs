
using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    private readonly PoolData _poolData;
    private Queue<GameObject> _objectPool;
    private readonly Transform _objectHolder;
 
    public Pool(PoolData poolData, Transform objectHolder)
    {
        _poolData = poolData;
        _objectHolder = objectHolder;
    }

    private void ConstructPool()
    {
        for (int i = 0; i < _poolData.poolSize; i++)
        {
            var objectInstance = Object.Instantiate(_poolData.gameObject, _objectHolder);
            objectInstance.SetActive(false);
            _objectPool.Enqueue(objectInstance);
        }
    }

    public GameObject RetrieveFromPool(Vector2 pos)
    {
        var obj = _objectPool.Dequeue();
        obj.transform.position = pos;
        obj.SetActive(true);
        return obj;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        _objectPool.Enqueue(obj);
    }
}
