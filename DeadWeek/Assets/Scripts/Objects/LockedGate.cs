using UnityEngine;
using System.Collections;

public class LockedGate : MonoBehaviour {
	bool triggered = false;
	Collider playerDetector;

	void OnTriggerEnter (Collider other) {
		Debug.Log ("Hit The Detector");
		triggered = true;
		playerDetector = other;
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	

			if (triggered && !playerDetector) {
				Debug.Log ("Should Destroy");
				Destroy (this.gameObject);
			}
		}
}
