using System.Collections;
using UnityEngine;

public class JailBubble : MonoBehaviour {
    [SerializeField] private float rePositionTime = 0.2f;
    [SerializeField] private int jailBreakCounter = 2;
 
    private Vector3 _center;
    private bool _inJail;
    private int _jailCounter;
    private ObjectPoolItem _objectPoolItem;

    private float _currentRePositionTime = 0.0f;

    private void OnEnable() {
        _center = GetComponent<Collider2D>().bounds.center;
        _objectPoolItem = GetComponent<ObjectPoolItem>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (IsTopSide(collision)) {
            // Move player to the center
            StartCoroutine(MovePlayerToCenter(collision.gameObject));
            _inJail = true;
        }
    }

    private void Update() {
        if (!_inJail) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _jailCounter++;
        }

        if (_jailCounter >= jailBreakCounter) {
            PlayerController.instance.Jump();

            ScoreManager.instance.AddScore(10);
            ScoreManager.instance.AddStreak();
            _objectPoolItem.CleanUp();
            Debug.Log("Collided with the top side");
            _inJail = false;
        }
    }

    private IEnumerator MovePlayerToCenter(GameObject gameObject) {
        Vector2 startingPosition = gameObject.transform.position;
        while (_currentRePositionTime < rePositionTime) {
            _currentRePositionTime += Time.deltaTime;
            float t = _currentRePositionTime / rePositionTime;
            Vector2 lerpPosition = Vector2.Lerp(startingPosition, _center, t);
            gameObject.transform.position = lerpPosition;
            yield return null;
        }
    }

    private bool IsTopSide(Collision2D collision) {
        Collider2D collider = collision.collider;
        Vector3 contactPoint = collision.contacts[0].point;
        Vector3 center = collider.bounds.center;

        Vector3 direction = contactPoint - center;
        direction.Normalize();

        if (direction.y < 0) {
            return true;
        }

        return false;
    }

    private void OnBecameInvisible() {
        _objectPoolItem.CleanUp();
    }
}
