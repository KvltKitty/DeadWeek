using UnityEngine;
using System.Collections;

public class PlaySound : MonoBehaviour {
	private AudioSource _source;
	void Start(){
		_source = gameObject.GetComponent<AudioSource>();
	}

	void playSound(AudioClip sound){
		_source.clip = sound;
		_source.Play();
	}
}
