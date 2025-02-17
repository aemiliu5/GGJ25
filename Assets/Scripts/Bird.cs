using System;
using UnityEngine;

public class Bird : MonoBehaviour
{
	public float speed;
	public bool movingRight;

	private void Start()
	{
		GetComponent<SpriteRenderer>().flipX = movingRight ? true : false;
		Destroy(gameObject, 15f);
	}

	private void Update()
	{
		transform.Translate(movingRight ? -speed : speed, 0, 0);
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		AudioManager.instance.PlaySoundOnce(AudioManager.instance.wingFlap);
		Destroy(gameObject);
		PlayerController.instance.ActivateBirdMode();
	}
}
