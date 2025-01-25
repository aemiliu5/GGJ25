using UnityEngine;
using Vector3 = System.Numerics.Vector3;

namespace Assets.Scripts.Pool
{
    public interface IPooler
    {
        void PullObjectFromPool(Vector3 pos);
        void ReturnToPull(GameObject gameObject);
    }
}