using UnityEngine;
using DG.Tweening;

public class LoseCollider : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log("LOSING");
		GameManager.instance.ChangeGameState(GameManager.GameState.LOST);

		MusicManager.instance.floriko.Stop();
		MusicManager.instance.metal.Stop();
		AudioManager.instance.PlaySoundOnce(AudioManager.instance.loseMusic);
		AudioManager.instance.PlaySoundOnce(AudioManager.instance.ghost);

		if(!PlayerController.instance.ghostSpawned)
			Instantiate(PlayerController.instance.ghostPrefab, collision.transform.position, Quaternion.identity);
		
		collision.gameObject.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => 
		{
			Destroy(collision.gameObject);
		});
	}
}
