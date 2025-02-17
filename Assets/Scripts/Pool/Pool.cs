
using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    private readonly PoolData _poolData;
    private Queue<GameObject> _objectPool;
    private readonly Transform _objectHolder;
 
    public Pool(PoolData poolData, Transform objectHolder)
    {
        _objectPool = new Queue<GameObject>();
        _poolData = poolData;
        _objectHolder = objectHolder;
        ConstructPool();
    }

    private void ConstructPool()
    {
        for (int i = 0; i < _poolData.poolSize; i++)
        {
            var objectInstance = Object.Instantiate(_poolData.gameObject, _objectHolder);
            objectInstance.SetActive(false);
        
            var poolItem = objectInstance.GetComponent<ObjectPoolItem>();
            if (poolItem == null)
            {
                Debug.LogError("Missing ObjectPoolItem component!");
                continue;
            }

            poolItem.Init(this);
            _objectPool.Enqueue(objectInstance);
        }
    }


    public GameObject RetrieveFromPool(Vector2 pos)
    {
        Debug.Log($"Retrieving from pool. Current size before dequeue: {_objectPool.Count}");

        if (_objectPool.Count == 0)
        {
            Debug.LogWarning("Pool is empty! Instantiating new object.");
            var newObj = Object.Instantiate(_poolData.gameObject, _objectHolder);
            newObj.GetComponent<ObjectPoolItem>().Init(this);
            newObj.transform.position = new Vector3(pos.x, pos.y, 0.0f);
            newObj.SetActive(true);
            return newObj;
        }

        var obj = _objectPool.Dequeue();
        obj.transform.position = new Vector3(pos.x, pos.y, 0.0f);
        obj.SetActive(true);
        Debug.Log($"Object retrieved. Pool size after dequeue: {_objectPool.Count}");

        return obj;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        _objectPool.Enqueue(obj);
        Debug.Log($"Returned {obj.name} to pool. Pool size after enqueue: {_objectPool.Count}");
    }



    public int Count()
    {
        return _objectPool.Count;
    }



}
