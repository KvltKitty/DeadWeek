using UnityEngine;
using System.Collections;

public class MansionDoorScript : MonoBehaviour {
	public Transform teleportLocation;
	private bool isOpen;
	// Use this for initialization
	void Start () {
		isOpen = false;
	}

	void openDoor(){
		isOpen = true;
	}
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag.Equals ("Player")){
			if(isOpen){
				other.gameObject.transform.parent.transform.position = teleportLocation.position;
				isOpen = false;
			}
		}
	}
}
