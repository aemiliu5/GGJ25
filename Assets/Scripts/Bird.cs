using System;
using UnityEngine;

public class Bird : MonoBehaviour
{
	public float speed;
	private void Update()
	{
		transform.Translate(speed, 0, 0);
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		Destroy(gameObject);
		PlayerController.instance.ActivateBirdMode();
	}
}
