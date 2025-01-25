using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float horizontalSpeed;
	public float jumpForce;

	public float leftBound;
	public float rightBound;

	private Rigidbody2D rb;

	public static PlayerController instance;

	private void Start()
	{
		if (instance != null)
			Destroy(instance.gameObject);

		instance = this;

		rb = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		Vector2 velocity = rb.linearVelocity;

		if (Input.GetKey(KeyCode.A) && transform.position.x > leftBound)
		{
			velocity.x = -horizontalSpeed;
		}
		else if (Input.GetKey(KeyCode.D) && transform.position.x < rightBound)
		{
			velocity.x = horizontalSpeed;
		}
		else
		{
			velocity.x = 0;
		}

		rb.linearVelocity = velocity;
	}
	
	// --- Jump ---
	
	public void Jump()
	{
		rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
	}

	public void InitialJump()
	{
		rb.AddForce(new Vector2(0, jumpForce * 2), ForceMode2D.Impulse);
	}
}