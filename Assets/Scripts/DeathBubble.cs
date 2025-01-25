using UnityEngine;

public class DeathBubble : MonoBehaviour {
    private ObjectPoolItem _objectPoolItem;

    private void OnEnable() {
        _objectPoolItem = GetComponent<ObjectPoolItem>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("LOSING");
        GameManager.instance.ChangeGameState(GameManager.GameState.LOST);
        Destroy(collision.gameObject);
    }

    private void OnBecameInvisible() {
        _objectPoolItem.CleanUp();
    }
}
