using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public enum JumpType { Normal, Jail, Yarn}

	public float horizontalSpeed;
	public float jumpForce;
	public float initialJumpForce;
	[SerializeField] private float jailJumpForce = 12;
	[SerializeField] private float yarnJumpForce = 12;

	public float leftBound;
	public float rightBound;

	private Animator anim;
	private Rigidbody2D rb;
	private SpriteRenderer sr;

	public static PlayerController instance;

	public bool IsInJail { get; set; }

	private Dictionary<JumpType, float> _jumpTypeToForce;

	private void Start()
	{
		if (instance != null)
			Destroy(instance.gameObject);

		instance = this;

		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();

		_jumpTypeToForce = new Dictionary<JumpType, float>() {
			{ JumpType.Normal, jumpForce },
			{ JumpType.Jail, jailJumpForce },
			{ JumpType.Yarn, yarnJumpForce },
		};
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
	
	public void Jump(JumpType jumpType = JumpType.Normal)
	{
		anim.SetTrigger("col");
		rb.linearVelocityY = 0;
		rb.AddForce(new Vector2(0, _jumpTypeToForce[jumpType]), ForceMode2D.Impulse);
	}

    public void InitialJump()
	{
		Debug.Log("Initial Jump called!");
		anim.SetTrigger("Initial_Jump");
	}

	public void LaunchCatUpwards()
	{
		Debug.Log("Will launch cat upwards!");
		rb.AddForce(new Vector2(0, initialJumpForce), ForceMode2D.Impulse);
	}

	public void Simulated(bool simulated) {
		rb.simulated = simulated;
	}

	public void TriggerYarn() {
		anim.SetTrigger("yarn");
	}

	public void TriggerJail() {
		anim.SetTrigger("jailed");
	}
}