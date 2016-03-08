using UnityEngine;
using System.Collections;

public class MansionKeyScript : MonoBehaviour {
	public GameObject doorToOpen;
	public AudioClip doorOpenSound;
	private MansionQuickDirtyReturn _return;
	// Use this for initialization
	void Start () {
		_return = gameObject.GetComponent<MansionQuickDirtyReturn>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter(Collider other){
		if(other.tag.Equals ("Player")){
			doorToOpen.SendMessage("openDoor");
			//setAudio player parented to player's clip to door open sound and play without
			if(_return.lobbyReturn != null)
			{
				other.transform.parent.transform.position = _return.lobbyReturn.transform.position;
			}
			Destroy (gameObject);
		}
	}
}
