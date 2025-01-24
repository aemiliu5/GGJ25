using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float horizontalSpeed;
    public float jumpForce;

    public float leftBound;
    public float rightBound;

    public static PlayerController instance;

    private void Start()
    {
        if (instance != null)
            Destroy(instance.gameObject);

        instance = this;
    }

    public void Jump()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    }

    private void Update() 
    {
        if (Input.GetKey(KeyCode.A) && transform.position.x > leftBound)
        {
            transform.Translate(-horizontalSpeed, 0, 0);
        }
        
        if (Input.GetKey(KeyCode.D) && transform.position.x < rightBound)
        {
            transform.Translate(horizontalSpeed, 0, 0);
        }
    }
}
