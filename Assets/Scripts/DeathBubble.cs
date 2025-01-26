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
            AudioManager.instance.PlaySoundOnce(AudioManager.instance.ghost);

            PlayerController.instance.GetComponent<BoxCollider2D>().enabled = false;
            
            if(!PlayerController.instance.ghostSpawned)
                Instantiate(PlayerController.instance.ghostPrefab, collision.transform.position, Quaternion.identity);
            
            collision.gameObject.transform.DOScale(Vector3.zero, 1f).OnComplete(() => 
            {
                Destroy(collision.gameObject);
            });
        }
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
