using UnityEngine;
public class ObjectPoolItem : MonoBehaviour {
    private Pool _owner;

    public void Init(Pool owner) {
        _owner = owner;
    }

    public void CleanUp() {
        _owner.ReturnToPool(gameObject);
    }
}
