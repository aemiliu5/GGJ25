using System;
using UnityEngine;

public class Bird : MonoBehaviour
{
	public float speed;
	private void Update()
	{
		transform.Translate(speed, 0, 0);
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		Destroy(gameObject);
	}
}
