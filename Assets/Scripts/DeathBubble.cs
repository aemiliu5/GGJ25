using UnityEngine;
using System.Collections;
public class DeathBubble : MonoBehaviour {
    [SerializeField] private float tweenDuration = 0.2f;
    private ObjectPoolItem _objectPoolItem;
    private Animator _animator;

    private float _currentTweenTime;

    private void OnEnable() {
        _objectPoolItem = GetComponent<ObjectPoolItem>();
        _animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (IsTopSide(collision)) {
            Debug.Log("LOSING");
            _animator.SetTrigger("explode");
            GameManager.instance.ChangeGameState(GameManager.GameState.LOST);
            StartCoroutine(TweenObject());
            Destroy(collision.gameObject);
        }
    }

    private IEnumerator TweenObject() {
        yield return new WaitForSeconds(1.5f);
        var startingScale = Vector2.one * 0.3f;
        while (transform.localScale.x > 0) {
            _currentTweenTime += Time.deltaTime;
            float t = _currentTweenTime / tweenDuration;
            transform.localScale = Vector2.Lerp(startingScale, Vector2.zero, t);
            yield return null;
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
