using System;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerCameraFollow : MonoBehaviour
{
	public Rigidbody2D playerBody;
	[SerializeField] private float followVelocitySensitivity;
	private CinemachineCamera camera;

	private float timer = 0f;
	private float timerEnd = 0.25f;
	private bool shouldFollow;

	private void Start()
	{
		camera = GetComponent<CinemachineCamera>();
	}

	private void Update()
	{
		if (playerBody.linearVelocityY > followVelocitySensitivity)
			timer = 0;
		
		timer += Time.deltaTime;
		
		shouldFollow = timer < timerEnd;
		camera.Follow = shouldFollow ? playerBody.transform : null;
	}
}
