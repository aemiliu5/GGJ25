using UnityEngine;
using System.Collections;
using DG.Tweening;

public class DeathBubble : MonoBehaviour {
    [SerializeField] private float tweenDuration = 0.2f;
    [SerializeField] private float playerTweenDuration = 0.3f;
    private ObjectPoolItem _objectPoolItem;
    private Animator _animator;

    private float _currentTweenTime;
    private float _currentPlayerTweenTime;

    private void OnEnable() {
        _objectPoolItem = GetComponent<ObjectPoolItem>();
        _animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (IsTopSide(collision)) {
            Debug.Log("LOSING");
            _animator.SetTrigger("explode");
            GameManager.instance.ChangeGameState(GameManager.GameState.LOST);

            MusicManager.instance.metal.Stop();
            MusicManager.instance.floriko.Stop();
            AudioManager.instance.PlaySoundOnce(AudioManager.instance.bomb);

            PlayerController.instance.GetComponent<BoxCollider2D>().enabled = false;

            collision.gameObject.transform.DOScale(Vector3.zero, 0.2f).SetDelay(0.2f).OnComplete(() => 
            {
                Destroy(collision.gameObject);
                
                if(!PlayerController.instance.ghostSpawned)
                    Instantiate(PlayerController.instance.ghostPrefab, collision.transform.position, Quaternion.identity);
                
                AudioManager.instance.PlaySoundOnce(AudioManager.instance.ghost);
            });
        }
    }

    private void Update()
    {
        if (GameManager.instance.currentGameState == GameManager.GameState.PLAY)
        {
            if (IsDownAndInvisible() && !_objectPoolItem.isBeingCleanedUp)
            { 
                CleanUp();
            }

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

        Debug.Log($"Returning {gameObject.name} to pool.");
        _objectPoolItem.CleanUp();
        
        _objectPoolItem.isBeingCleanedUp = false;
    }
}
