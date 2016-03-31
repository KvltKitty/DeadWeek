using UnityEngine;
using System.Collections;

public class PlayerInputController : MonoBehaviour {

    public PlayerInput Current;

    private SuperCharacterController _controller;
    private Player _player;

    private bool hiding;
    private bool canInteract;
    private Vector3 curInteractPos;
    private Vector3 enterPos;

    public bool getHiding() { return hiding; }

	// Use this for initialization
	void Start ()
    {
        _controller = transform.GetComponent<SuperCharacterController>();
        _player = transform.GetChild(1).GetComponent<Player>();
        Current = new PlayerInput();
        hiding = false;
        canInteract = false;
	}
	
	// Update is called once per frame
	void Update ()
    {

        // Retrieve our current WASD or Arrow Key input
        // Using GetAxisRaw removes any kind of gravity or filtering being applied to the input
        // Ensuring that we are getting either -1, 0 or 1
        Vector3 moveInput;
        Vector3 mouseInput;
        bool jumpInput;
        if (hiding)
        {
            moveInput = Vector3.zero;
            mouseInput = Vector3.zero;
            jumpInput = false;
            bool interactInput = Input.GetButtonDown("Fire3");
            if (interactInput)
            {
                hiding = false;
                enterPos.y = 0.5f;
                transform.position = enterPos;
                _controller.enabled = true;
                canInteract = false;
            }
        }
        else
        {
            moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            jumpInput = false;
        }

        if (canInteract)
        { 
            bool interactInput = Input.GetButtonDown("Fire3");
            if (interactInput)
            {
                hiding = true;
                enterPos = transform.position;
                _controller.enabled = false;
                transform.position = curInteractPos;
                canInteract = false;
            }
        }

        Current = new PlayerInput()
        {
            MoveInput = moveInput,
            MouseInput = mouseInput,
            JumpInput = jumpInput
        };
	}

    public void setInteract(bool interact, Vector3 objPos)
    {
        canInteract = interact;
        curInteractPos = objPos;
    }
}

public struct PlayerInput
{
    public Vector3 MoveInput;
    public Vector2 MouseInput;
    public bool JumpInput;
    public bool interactInput;
}
