using System.Collections;
using UnityEngine;

public class CustomSpriteAnim : MonoBehaviour
{
	[SerializeField] private Sprite[] sprites;
	[SerializeField] private float frameRate;

	private ObjectPoolItem _objectPoolItem;
	private SpriteRenderer _spriteRenderer;
	
	private int _currentIndex;

	private void OnEnable()
	{
		_objectPoolItem = GetComponent<ObjectPoolItem>();
		_spriteRenderer = GetComponent<SpriteRenderer>(); 
	}

	public void PopAnim()
	{
		StartCoroutine(PlayAnimRoutine());
	}

	public void ResetAnim()
	{
		_spriteRenderer.sprite = sprites[0];
	}

	private IEnumerator PlayAnimRoutine() {
		while (_currentIndex < sprites.Length) {
			_spriteRenderer.sprite = sprites[_currentIndex];
			yield return new WaitForSeconds(frameRate);
			_currentIndex++;
		}
		
		_objectPoolItem.CleanUp();
	}
}
