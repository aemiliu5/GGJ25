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
		if (player != null && player.transform.position.y - lastYPosition >= threshold)
		{
			CheckForSpawn();
		}
	}

	private void CheckForSpawn()
	{
		lastYPosition = player.transform.position.y;

		if (Random.value <= spawnChance)
		{
			bool movingRight = IntToBool(Random.Range(0, 2));
			float x = movingRight ? player.transform.position.x - 5 : player.transform.position.x + 5;
			Vector3 spawnPosition = new Vector3(x, player.transform.position.y + spawnHeight, 0);
			GameObject bird = Instantiate(specificPrefab, spawnPosition, Quaternion.identity);
			Bird birdComponent = bird.GetComponent<Bird>();
			birdComponent.movingRight = movingRight;
			birdComponent.speed *= Random.Range(1f, 1.5f);
			AudioManager.instance.PlaySoundOnce(AudioManager.instance.bird);
		}
	}

	private static bool IntToBool(int i)
	{
		return i != 0;
	}
}