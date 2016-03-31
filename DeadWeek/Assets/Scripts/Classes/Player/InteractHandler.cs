using UnityEngine;
using System.Collections;

public class InteractHandler : MonoBehaviour {
    private PlayerInputController _controller;

    private bool _canInteract;
	// Use this for initialization
	void Start ()
    {
        _controller = transform.parent.parent.GetComponent<PlayerInputController>();
    }
	
	// Update is called once per frame
	void Update ()
    {
         
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Interact"))
        {
            _canInteract = true;
            _controller.setInteract(true, other.transform.position);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Interact"))
        {
            _canInteract = false;
            _controller.setInteract(false, other.transform.position);
        }
    }
}
