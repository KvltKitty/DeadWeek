using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class Player : MonoBehaviour 
{
	public GameObject ball;
	TrajectorySimulation _simulation;
	public Ability _abilities;
	private bool hasKey;
	private bool casting;
	private bool fireBall;
	private float startCastTime;
	private float power;
	private float lastCalculateTime;
	private float calculateInterval = 0.001f;
	private float radius = 0.5f;

	// Reference to the LineRenderer we will use to display the simulated path
	public LineRenderer sightLine;
	
	public float ballSpeed;
	public float maxDistanceToGoal = 0.01f;
	
	// Reference to a Component that holds information about fire strength, location of cannon, etc.
	private bool finished = false;
	private float strength;

	private float skinWidth = 0.005f;
	// Number of segments to calculate - more gives a smoother line
	public int segmentCount = 20;
	
	// Length scale for each segment
	public float segmentScale = 0.009f;

	// gameobject we're actually pointing at (may be useful for highlighting a target, etc.)
	private Collider _hitObject;
	public Collider hitObject { get { return _hitObject; } }

	private float inputStrength;
	private int numBalls;

    private Transform interact;

	void Start()
	{
		numBalls = 3;
		inputStrength = 0.0f;
		fireBall = false;
		lastCalculateTime = 0.0f;
		numBalls = 3;
		casting = false;
		sightLine = gameObject.GetComponent<LineRenderer>();
        interact = transform.GetChild(1);
        Debug.Log(interact.gameObject.name);
	}

	void FixedUpdate()
	{
		if(fireBall)
		{
			FireBall();
			fireBall = false;
			sightLine.enabled = false;
		}
		if(casting)
		{
			sightLine.enabled = true;
			if(Time.time - lastCalculateTime >= calculateInterval)
			{
				strength = ((inputStrength % 1.0f) + 1.0f) * 500.0f;
				simulatePath (strength);
				lastCalculateTime = Time.time;
			}
		}
	}

	void Update()
	{
		if(numBalls > 0){
		if(Input.GetAxisRaw ("Fire1") == 1.0f)
		{
			startCastTime = Time.time;
			casting = true;
		}
            inputStrength = 140.0f;
        /*
		inputStrength -= Input.GetAxis ("Mouse Y")/50.0f;
		if(inputStrength > 0.99f){
			inputStrength = 0.99f;
				Debug.Log ("Input strength set to 1");
		}
		if(inputStrength < 0.0f){
			inputStrength = 0.0f;
				Debug.Log ("Input strength set to 0");
		}
        */
		if(casting){
		if(Input.GetAxisRaw ("Fire1") == 0.0f)
		{
			fireBall = true;
			casting = false;
					numBalls--;
		}
		}
		}

	}

	void LateUpdate()
	{

	}

	void Reset()
	{
	
	}

	void FireBall()
	{
		GameObject _ball = Instantiate(ball, interact.transform.position, transform.rotation) as GameObject;
		_ball.SendMessage("setFireStrength", strength);
	}

	void OnTriggerEnter (Collider other) {
		Debug.Log ("Hit");
		if (other.gameObject.tag == "Key") {
			Debug.Log ("Hit Key");
			hasKey = true;
			Destroy (other.gameObject);
		}
		if(other.gameObject.tag.Equals ("Ball")){
			Destroy (other.gameObject);
			numBalls++;
		}
	}

	void simulatePath(float strength)
	{
		int bounceCount = 0;
		int j = 0;
		segmentCount = 0;
		GameObject _temp = new GameObject();
		_temp.transform.position = interact.position;
		_temp.transform.rotation = transform.rotation;
		_temp.transform.Rotate (Vector3.right * 45.0f, Space.Self);
		Vector3 fireRotation = _temp.transform.up;


		List<Vector3> segments = new List<Vector3>();

		// The first line point is wherever the player's cannon, etc is
		segments.Add (_temp.transform.position);
		segmentCount++;
		j++;
		// The initial velocity
		Vector3 segVelocity = fireRotation * strength * Time.deltaTime;

		// reset our hit object
		_hitObject = null;
		while(bounceCount < 3)
		{
			if(segmentCount > 7000){
				Debug.Log ("YOU FUCKED UP!");
				Debug.Log (segments[segments.Count - 1].y);
				break;
			}
			// Time it takes to traverse one segment of length segScale (careful if velocity is zero)
			float segTime = (segVelocity.sqrMagnitude != 0) ? segmentScale / segVelocity.magnitude : 0;
			// Add velocity from gravity for this segment's timestep
			segVelocity = segVelocity + Physics.gravity * segTime;
			
			// Check to see if we're going to hit a physics object
			RaycastHit hit;

			if (Physics.Raycast(segments[j - 1], segVelocity, out hit, segmentScale + skinWidth))
				{
					bounceCount++;
					segVelocity *= 0.60f;
					//Debug.Log (hit.distance);
					Debug.DrawRay (segments[j - 1], segVelocity);
					// remember who we hit
					_hitObject = hit.collider;
					if(hit.distance <= skinWidth){
						segments.Add (hit.point + (Vector3.up * (skinWidth + 0.001f)));
						segVelocity = segVelocity - Physics.gravity * (segmentScale - hit.distance) / segVelocity.magnitude;
						segVelocity = Vector3.Reflect(segVelocity, hit.normal);
					}
					else{
						// set next position to the position where we hit the physics object
						segments.Add (segments[j - 1] + segVelocity.normalized * hit.distance);
						// correct ending velocity, since we didn't actually travel an entire segment
						segVelocity = segVelocity - Physics.gravity * (segmentScale - hit.distance) / segVelocity.magnitude;
						// flip the velocity to simulate a bounce
						segVelocity = Vector3.Reflect(segVelocity, hit.normal);
					}
					
					/*
				 * Here you could check if the object hit by the Raycast had some property - was 
				 * sticky, would cause the ball to explode, or was another ball in the air for 
				 * instance. You could then end the simulation by setting all further points to 
				 * this last point and then breaking this for loop.
				 */
				}
				
				// If our raycast hit no objects, then set the next position to the last one plus v*t
				else
				{
					segments.Add (segments[j - 1] + segVelocity * segTime);
				}
			segmentCount++;
			j++;
		}
		
		// At the end, apply our simulations to the LineRenderer
		
		// Set the colour of our path to the colour of the next ball
		//Color startColor = playerFire.nextColor;
		//Color endColor = startColor;
		//startColor.a = 1;
		//endColor.a = 0;
		//sightLine.SetColors(startColor, endColor);
		
		sightLine.SetVertexCount(segmentCount);
		for (int i = 0; i < segmentCount; i++)
		{
			sightLine.SetPosition(i, segments[i]);
		}
		Destroy (_temp.gameObject);
		
	}


}


