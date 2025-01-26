using System;
using System.Collections;
using UnityEngine;

public class YarnBubble : MonoBehaviour {
    [SerializeField] private float spinTime = 1.5f;
    [SerializeField] private float repositionTime = 0.2f;

    private Vector3 _center;
    private ObjectPoolItem _objectPoolItem;

    private PlayerController _playerController;

    private float _currentRePositionTime = 0.0f;

    private void OnEnable() {
        _center = GetComponent<Collider2D>().bounds.center;
        _objectPoolItem = GetComponent<ObjectPoolItem>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        _playerController = collision.gameObject.GetComponent<PlayerController>();
        StartCoroutine(MovePlayerToCenter(collision.gameObject, () => {
            _playerController.TriggerYarn();
            StartCoroutine(LaunchAfterTime());
        }));

        _playerController.Simulated(false);
        _playerController.IsInJail = true;
    }

    private IEnumerator LaunchAfterTime() {
        yield return new WaitForSeconds(spinTime);
        ScoreManager.instance.AddScore(100);
        _playerController.Jump(PlayerController.JumpType.Yarn);
        _playerController.Simulated(true);
        _playerController.IsInJail = false;
        _objectPoolItem.CleanUp();
    }

    private IEnumerator MovePlayerToCenter(GameObject gameObject, Action onComplete) {
        Vector2 startingPosition = gameObject.transform.position;
        while (_currentRePositionTime < repositionTime) {
            _currentRePositionTime += Time.deltaTime;
            float t = _currentRePositionTime / repositionTime;
            Vector2 lerpPosition = Vector2.Lerp(startingPosition, _center, t);
            gameObject.transform.position = lerpPosition;
            yield return null;
        }

        onComplete?.Invoke();
    }

    private void Update()
    {
        if (GameManager.instance.currentGameState == GameManager.GameState.PLAY)
        {
            if(IsDownAndInvisible())
                _objectPoolItem.CleanUp();
        }
    }

    private bool IsDownAndInvisible()
    {
        Vector3 myPos = transform.position;
        Vector3 playerPos = PlayerController.instance.transform.position;
        float threshold = 30f;
        
        return Vector2.Distance(myPos, playerPos) > threshold && myPos.y < playerPos.y;
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
}
