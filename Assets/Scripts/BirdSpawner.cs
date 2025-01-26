using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
	public GameObject player;
	public GameObject specificPrefab;

	public float lastYPosition;
	public float threshold = 20f;
	public float spawnHeight = 10f;
	public float spawnChance = 0.3f; // 30% chance

	private void Start()
	{
		if (player != null)
		{
			lastYPosition = player.transform.position.y;
		}
	}

	private void Update()
	{
		if (player != null)
		{
			CheckForSpawn();
		}
	}

	private void CheckForSpawn()
	{
		if (player.transform.position.y - lastYPosition >= threshold)
		{
			lastYPosition = player.transform.position.y;

			if (Random.value <= spawnChance)
			{
				Vector3 spawnPosition = new Vector3(player.transform.position.x, player.transform.position.y + spawnHeight, 0);
				Instantiate(specificPrefab, spawnPosition, Quaternion.identity);
				AudioManager.instance.PlaySoundOnce(AudioManager.instance.bird);
			}
		}
	}
}