using UnityEngine;
using System.Collections;


/* Give this controller to the PlayerMesh
 * This Class handels all the trigger collisions and all variables assosiated with trigger collisions
 * 
 * Handels the Key and Player collision system, Player and Gate collision system, Notes and Player 
 * Collision system...
 * 
 * Handels the Notes GUI so players can read them 
 */
public class ObjectController : MonoBehaviour {

	public bool _hasKey = false;
	public bool _displayNote1 = false;
	public bool _displayNote2 = false;
	public bool _displayNote3 = false;
	public int _randomNumber1;
	public int _randomNumber2;
	public int _randomNumber3;
	

	void Start () {
		// Initilizes all the random numbers, making sure that there are no repeats
		_randomNumber1 = Random.Range (1, 7);

		_randomNumber2 = Random.Range (1, 7);
		// while the first random number is the same as the second, get a different random number
		while (_randomNumber1 == _randomNumber2) {
			_randomNumber2 = Random.Range (1, 7);
		}

		_randomNumber3 = Random.Range (1, 7);
		// While the third random number is the same as the second or the first,
		// get a different random number
		while (_randomNumber3 == _randomNumber1 || _randomNumber3 == _randomNumber2) {
			_randomNumber3 = Random.Range (1, 7);
		}
	}

	
	
	void OnTriggerEnter (Collider other) {
		/*
		 * How to use the key:
		 * 	Create a shape
		 * 	Make it a rigid body
		 * 		make sure gravity is unchecked and kinimatics is checked
		 *	Give it the tag "Key"
		 *	Place the key where it's needed to be in the level
		 */

		// If colliding with the key, the player has the key and the key dissappears from the world
		if (other.gameObject.tag == "Key") {
			_hasKey = true;
			Destroy (other.gameObject);
			
		}

		/*
		 * How to use the gate:
		 * 	Create two shapes 
		 * 		one not visiable to the player that the player will collide with
		 * 		one visiable to the player that the player will not be able to pass until a key is found
		 * 	Give the transparent shape a rigid body and the tag named "Gate"
		 * 		make sure gravity is unchecked and kinimatics is checked
		 * 	Give the other shape the LockedGate script
		 * 	Place the solid shape where you want the gate to be
		 * 	Place the transparent shape so it's colliding with the solid shape, but also so the player
		 * can collide with it
		 * 
		 * see LockedGate script for more info
		 */

		//if the player is colliding with the transparent gate object and the player has a key
		// then destroy the transparent gate object (This will lead to the gate being destroied)
		if (other.gameObject.tag == "Gate") {
			if (_hasKey) {
				Destroy (other.gameObject);
			}
		}


		if (other.gameObject.tag == "Note1") {
			_displayNote1 = true;
		}
		
		if (other.gameObject.tag == "Note2") {
			_displayNote2 = true;
		}
		
		if (other.gameObject.tag == "Note3") {
			_displayNote3 = true;
		}
	}
	
	void OnTriggerExit (Collider other) {
		if (other.gameObject.tag == "Note1") {
			_displayNote1 = false;
		}
		
		if (other.gameObject.tag == "Note2") {
			_displayNote2 = false;
		}
		
		if (other.gameObject.tag == "Note3") {
			_displayNote3 = false;
		}
	}
	
	void OnGUI() {
		if (_displayNote1) {
			GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 200f, 200f), _randomNumber1.ToString());
		}
		if (_displayNote2) {
			GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 200f, 200f), _randomNumber2.ToString());
		}
		if (_displayNote3) {
			GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 200f, 200f), _randomNumber3.ToString());
		}
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
