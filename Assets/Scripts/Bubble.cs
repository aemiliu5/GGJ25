using System;
using UnityEngine;
using Random = System.Random;

public class Bubble : MonoBehaviour
{
    private ObjectPoolItem _objectPoolItem;
    public Sprite initialSprite;
    private SpriteRenderer spriteRenderer;
    private CustomSpriteAnim customSpriteAnim;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        customSpriteAnim = GetComponent<CustomSpriteAnim>();
        initialSprite = spriteRenderer.sprite;
    }

    private void OnEnable() {
        _objectPoolItem ??= GetComponent<ObjectPoolItem>();
        if(customSpriteAnim != null)
            customSpriteAnim.ResetAnim();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Collision detected");

        Collider2D collider = collision.collider;
        Vector3 contactPoint = collision.contacts[0].point;
        Vector3 center = collider.bounds.center;

        //Debug.Log("Contact Point: " + contactPoint);
        //Debug.Log("Center: " + center);

        Vector3 direction = contactPoint - center;
        direction.Normalize();

        //Debug.Log("Direction: " + direction);

        if (direction.y < 0)
        {
            int random = UnityEngine.Random.Range(0, 2);
            AudioManager.instance.PlaySoundOnce(random == 1 ? AudioManager.instance.popBubble : AudioManager.instance.popBubble2);
            PlayerController.instance.Jump();
            ScoreManager.instance.AddScore(10);
            ScoreManager.instance.AddStreak();
            customSpriteAnim.PopAnim();
        }
    }

    private void Update()
    {
        if (GameManager.instance.currentGameState == GameManager.GameState.PLAY)
        {
            if(IsDownAndInvisible() && !_objectPoolItem.isBeingCleanedUp)
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
