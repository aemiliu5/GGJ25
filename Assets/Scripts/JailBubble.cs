using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class JailBubble : MonoBehaviour {
    [SerializeField] private float rePositionTime = 0.2f;
    [SerializeField] private int jailBreakCounter = 2;
 
    private Vector3 _center;
    private int _jailCounter;
    private ObjectPoolItem _objectPoolItem;

    private PlayerController _playerController;
    private Camera _mainCamera;
    private float _currentRePositionTime = 0.0f;

    private void OnEnable() {
        //_playerController = PlayerController.instance;
        _center = GetComponent<Collider2D>().bounds.center;
        _objectPoolItem = GetComponent<ObjectPoolItem>();
        _mainCamera = Camera.main;
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
        if (GameManager.instance.currentGameState == GameManager.GameState.PLAY)
        {
            if (IsDownAndInvisible() && !_objectPoolItem.isBeingCleanedUp)
            {
                CleanUp();
            }
        }
        
        if (_playerController == null) return;
        

        
        if (!_playerController.IsInJail) return;

        #if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_WEBGL
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _jailCounter++;
        }
        #elif UNITY_ANDROID
        if (Input.touches.Length == 1)
        {
            _jailCounter++;
        }
        #endif
        
        if (_playerController.IsInJail)
        {
            _playerController.transform.Translate(0, -1f * Time.deltaTime, 0);
            transform.Translate(0, -1f * Time.deltaTime, 0);
            FindObjectOfType<Light2D>().color -= new Color(0.0005f, 0.001f, 0.001f, 0f);

            if (_playerController.transform.position.y < _mainCamera.transform.position.y - 8f)
                GameManager.instance.ChangeGameState(GameManager.GameState.LOST);
        }

        if (_jailCounter >= jailBreakCounter) {
            PlayerController.instance.Jump(PlayerController.JumpType.Jail);
            ScoreManager.instance.ResetStreak();
            Debug.Log("Collided with the top side");
            _playerController.Simulated(true);
            _playerController.IsInJail = false;
            FindObjectOfType<Light2D>().color = new Color(1f, 1f, 1f, 0f);
            CleanUp();
        }
    }

    private bool IsDownAndInvisible()
    {
        float myY = transform.position.y;
        float playerY = PlayerController.instance.transform.position.y;
        float threshold = 20f;

        float distance = Mathf.Abs(myY - playerY); // Distance along the Y-axis
        
        return distance > threshold && myY < playerY;
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
    
    public void CleanUp()
    {
        if (_objectPoolItem == null)
        {
            Debug.LogError($"Bubble {gameObject.name} has no ObjectPoolItem reference!");
            Destroy(gameObject);
            return;
        }

        if (_objectPoolItem.isBeingCleanedUp)
        {
            Debug.LogWarning($"{gameObject.name} is already being cleaned up!");
            return;
        }

        _objectPoolItem.isBeingCleanedUp = true;
        _playerController = null;
        _currentRePositionTime = 0;
        
        Debug.Log($"Returning {gameObject.name} to pool.");
        _objectPoolItem.CleanUp();
        
        _objectPoolItem.isBeingCleanedUp = false;
    }
}
