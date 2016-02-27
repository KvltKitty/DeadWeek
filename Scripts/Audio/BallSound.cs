using UnityEngine;
using System.Collections;

public class BallSound : MonoBehaviour {
	private AudioSource _audioSource;
	public AudioClip[] _bounces = new AudioClip[3];

	private EmitSound _emitSound;
	private bool hasPlayed;
	// Use this for initialization
	void Start () 
	{

		hasPlayed = false;
		_emitSound = gameObject.GetComponent<EmitSound>();
		/*for(int i = 0; i < _bounces.Length; i++)
		{
			string loadFile = "SoundFX/Ball/BallHitWood" + (i + 1) + "-1";
			_bounces[i] = Resources.Load (loadFile) as AudioClip;
		}*/

	}
	
	// Update is called once per frame
	void Update () 
	{
	
			if(!_audioSource.isPlaying)
			{
				if(!hasPlayed)
				{
					_audioSource.Play ();
					_emitSound.toggleSoundPlaying();
					hasPlayed = true;
				}
				else if(hasPlayed)
				{
					Destroy (gameObject);
				}
			}

	}

	void fireSound(int indexNumber)
	{
		_audioSource.clip = _bounces[indexNumber];
		_audioSource.Play();
	}
	void initializeVariables(int indexNumber){
		_audioSource = gameObject.GetComponent<AudioSource>();
		_audioSource.clip = _bounces[indexNumber];

	}

}
