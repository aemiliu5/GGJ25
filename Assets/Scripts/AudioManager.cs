using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public AudioClip popBubble;
	public AudioClip popBubble2;
	public AudioClip jailBubble;
	public AudioClip UITick;
	public AudioClip bird;
	public AudioClip ghost;
	public AudioClip bomb;
	public AudioClip excited;
	public AudioClip purr;
	public AudioClip loseMusic;
	
	public static AudioManager instance;

	private void Start()
	{
		instance = this;
	}

	public void PlaySoundOnce(AudioClip clip)
	{
		GameObject soundObject = new GameObject();
		AudioSource audioSource = soundObject.AddComponent<AudioSource>();
		audioSource.PlayOneShot(clip);
		soundObject.transform.position = Camera.main.transform.position;
		Destroy(soundObject, clip.length);
	}
}
