using System;
using System.Collections;
using UnityEngine;

public class JailBubble : MonoBehaviour {
    [SerializeField] private float rePositionTime = 0.2f;
    [SerializeField] private int jailBreakCounter = 2;
 
    private Vector3 _center;
    private int _jailCounter;
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
            _playerController.TriggerJail();
        }));
        _playerController.Simulated(false);
        _playerController.IsInJail = true;
    }

    private void Update() {
        if (_playerController == null) return;
        if (!_playerController.IsInJail) return;

        #if UNITY_ANDROID
        if (Input.touches.Length == 1)
        {
            _jailCounter++;
        }
        #endif

        if (_jailCounter >= jailBreakCounter) {
            PlayerController.instance.Jump(PlayerController.JumpType.Jail);

            ScoreManager.instance.AddScore(10);
            ScoreManager.instance.ResetStreak();
            _objectPoolItem.CleanUp();
            Debug.Log("Collided with the top side");
            _playerController.Simulated(true);
            _playerController.IsInJail = false;
        }

        if (_playerController.IsInJail)
        {
            _playerController.transform.Translate(0, -0.5f * Time.deltaTime, 0);
            transform.Translate(0, -0.5f * Time.deltaTime, 0);
        }
        
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

    private IEnumerator MovePlayerToCenter(GameObject gameObject, Action onComplete) {
        Vector2 startingPosition = gameObject.transform.position;
        while (_currentRePositionTime < rePositionTime) {
            _currentRePositionTime += Time.deltaTime;
            float t = _currentRePositionTime / rePositionTime;
            Vector2 lerpPosition = Vector2.Lerp(startingPosition, _center, t);
            gameObject.transform.position = lerpPosition;
            yield return null;
        }

        onComplete?.Invoke();
    }
}
