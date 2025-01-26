using UnityEngine;
using DG.Tweening;

public class LoseCollider : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log("LOSING");
		GameManager.instance.ChangeGameState(GameManager.GameState.LOST);
		Instantiate(PlayerController.instance.ghostPrefab, collision.transform.position, Quaternion.identity);
		collision.gameObject.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => 
		{
			Destroy(collision.gameObject);
		});
	}
}
