using System;
using UnityEngine;

public class ButtonEffect : MonoBehaviour
{
	[SerializeField] private float amplitude = 1f; // Height of the oscillation
	[SerializeField] private float frequency = 1f; // Speed of the oscillation
	[SerializeField] private float offset = 0f; // Phase offset
	private Vector3 startPos;

	private void Start()
	{
		startPos = transform.position; // Store the initial position
	}

	private void Update()
	{
		float newY = startPos.y + Mathf.Sin(Time.time * frequency + offset) * amplitude;
		transform.position = new Vector3(startPos.x, newY, startPos.z);
	}
}