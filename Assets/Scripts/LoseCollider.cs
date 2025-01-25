using UnityEngine;

public class LoseCollider : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log("LOSING");
		GameManager.instance.ChangeGameState(GameManager.GameState.LOST);
		Destroy(collision.gameObject);
	}
}
