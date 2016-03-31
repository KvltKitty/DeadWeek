using UnityEngine;
using System.Collections;

public class InteractObject : MonoBehaviour {
    private int _type;
	// Use this for initialization
	void Start ()
    {
        _type = gameObject.layer;
	}
	
	// Update is called once per frame
	public void Interact (GameObject player)
    {
        //if is hiding object
	    if(_type == 9)
        {
           
        }
	}
}
