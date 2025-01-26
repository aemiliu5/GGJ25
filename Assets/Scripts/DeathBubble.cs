using UnityEngine;
using System.Collections;
public class DeathBubble : MonoBehaviour {
    [SerializeField] private float tweenDuration = 0.2f;
    [SerializeField] private float playerTweenDuration = 0.3f;
    private ObjectPoolItem _objectPoolItem;
    private Animator _animator;

    private float _currentTweenTime;
    private float _currentPlayerTweenTime;

    private GameObject _playerObject;

    private void OnEnable() {
        _objectPoolItem = GetComponent<ObjectPoolItem>();
        _animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (IsTopSide(collision)) {
            Debug.Log("LOSING");
            _playerObject = collision.gameObject;
            _animator.SetTrigger("explode");
            GameManager.instance.ChangeGameState(GameManager.GameState.LOST);
            StartCoroutine(TweenPlayer());
            StartCoroutine(TweenObject());
            Destroy(collision.gameObject);
        }
    }

    //TODO:: DON'T HARD CODE THIS - MOVE IT TO THE PLAYER CONTROLLER

    private IEnumerator TweenPlayer() {
        var startingScale = _playerObject.transform.localScale;
        while (_playerObject.transform.localScale.x > 0) {
            _currentPlayerTweenTime += Time.deltaTime;
            float t = _currentPlayerTweenTime / playerTweenDuration;
            transform.localScale = Vector2.Lerp(startingScale, Vector2.zero, t);
            yield return null;
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

    private void Update()
    {
        if(IsDownAndInvisible())
            _objectPoolItem.CleanUp();
    }
    

    private bool IsDownAndInvisible()
    {
        Vector3 myPos = transform.position;
        Vector3 playerPos = PlayerController.instance.transform.position;
        float threshold = 30f;
        
        return Vector2.Distance(myPos, playerPos) > threshold && myPos.y < playerPos.y;
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
