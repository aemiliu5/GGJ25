using UnityEngine;
using Unity.Cinemachine;

[AddComponentMenu("")] // Hide in menu
[SaveDuringPlay]
[ExecuteInEditMode]
public class LockCameraExtension : CinemachineExtension
{
	protected enum LockDirection
	{
		X,
		Y,
		Z
	}

	[Header("Settings")]
	[Tooltip("direction to lock the camera's position")]
	[SerializeField] LockDirection lockDirection = LockDirection.Z;
	[Tooltip("lock the camera's position to this value")]
	public float lockPositionValue = 10;
	[Tooltip("whether the lockPositionValue's origin is the VCam's follow transform")]
	[SerializeField] bool useVCamFollowAsOrigin;

	protected override void PostPipelineStageCallback(
		CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
	{
		if (stage != CinemachineCore.Stage.Body) return;
       
		Vector3 pos = state.RawPosition;
		Vector3 lockPosition = (useVCamFollowAsOrigin && vcam.Follow ? vcam.Follow.position : Vector3.zero) +
		                       Vector3.one * lockPositionValue;
       
		state.RawPosition = lockDirection switch
		{
			LockDirection.X => new Vector3(lockPosition.x, pos.y, pos.z),
			LockDirection.Y => new Vector3(pos.x, lockPosition.y, pos.z),
			LockDirection.Z => new Vector3(pos.x, pos.y, lockPosition.z),
			_ => throw new System.NotImplementedException()
		};
	}
}