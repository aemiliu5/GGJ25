using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float horizontalSpeed;
	public float jumpForce;

	public float leftBound;
	public float rightBound;

	private Animator anim;
	private Rigidbody2D rb;
	private SpriteRenderer sr;

	public static PlayerController instance;

	public bool IsInJail { get; set; }

	private void Start()
	{
		if (instance != null)
			Destroy(instance.gameObject);

		instance = this;

		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();
	}

	private void Update() {
        if (IsInJail) { return; }

        Vector2 velocity = rb.linearVelocity;

		if (Input.GetKey(KeyCode.A) && transform.position.x > leftBound)
		{
			velocity.x = -horizontalSpeed;
			sr.flipX = true;
		}
		else if (Input.GetKey(KeyCode.D) && transform.position.x < rightBound)
		{
			velocity.x = horizontalSpeed;
			sr.flipX = false;
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
		anim.SetTrigger("col");
		rb.linearVelocityY = 0;
		rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
	}

	public void InitialJump()
	{
		Debug.Log("Initial Jump called!");
		anim.SetTrigger("Initial_Jump");
		rb.AddForce(new Vector2(0, jumpForce * 2), ForceMode2D.Impulse);
	}

	public void Simulated(bool simulated) {
		rb.simulated = simulated;
	}
}