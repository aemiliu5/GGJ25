using System;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerCameraFollow : MonoBehaviour
{
	public Rigidbody2D playerBody;
	private CinemachineCamera camera;

	private void Start()
	{
		camera = GetComponent<CinemachineCamera>();
	}

	private void Update()
	{
		camera.Follow = (playerBody.linearVelocityY > 0.5f) ? playerBody.transform : null;
		
		Debug.Log(playerBody.linearVelocityY);
	}
	
	
}
