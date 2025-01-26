using System;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    private ObjectPoolItem _objectPoolItem;
    public Sprite initialSprite;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialSprite = spriteRenderer.sprite;
    }

    private void OnEnable() {
        _objectPoolItem ??= GetComponent<ObjectPoolItem>();
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
            PlayerController.instance.Jump();
            ScoreManager.instance.AddScore(10);
            ScoreManager.instance.AddStreak();
            GetComponent<CustomSpriteAnim>().PopAnim();
        }
    }

    private void OnBecameInvisible() {
        initialSprite = spriteRenderer.sprite;
        CleanUp();
    }

    private void CleanUp()
    {
        _objectPoolItem?.CleanUp();
    }
}
