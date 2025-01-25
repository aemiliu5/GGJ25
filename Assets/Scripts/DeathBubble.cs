using UnityEngine;
public class DeathBubble : MonoBehaviour {
    private ObjectPoolItem _objectPoolItem;

    private void OnEnable() {
        _objectPoolItem = GetComponent<ObjectPoolItem>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (IsTopSide(collision)) {
            Debug.Log("LOSING");
            GameManager.instance.ChangeGameState(GameManager.GameState.LOST);
            Destroy(collision.gameObject);
        }
    }

    private void OnBecameInvisible() {
        _objectPoolItem.CleanUp();
    }

    private bool IsTopSide(Collision2D collision)
    {
        Collider2D collider = collision.collider;
        Vector3 contactPoint = collision.contacts[0].point;
        Vector3 center = collider.bounds.center;

        Vector3 direction = contactPoint - center;
        direction.Normalize();

        if (direction.y < 0)
        {
            return true;
        }

        return false;
    }
}
