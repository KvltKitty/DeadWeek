using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Controls the Laser Sight for the player's aim
/// </summary>
public class TrajectorySimulation : MonoBehaviour
{
	public GameObject ballSound;	
	// Reference to the LineRenderer we will use to display the simulated path
	public LineRenderer sightLine;

	public int ballSpeed;
	public float maxDistanceToGoal = 0.1f;
	
	// Reference to a Component that holds information about fire strength, location of cannon, etc.
	public PlayerFire playerFire;
	private bool finished = false;

	private float skinWidth = 0.005f;
	
	// Number of segments to calculate - more gives a smoother line
	public int segmentCount = 20;
	
	// Length scale for each segment
	public float segmentScale = 0.009f;
	private float radius;
	private List<Vector3> path;
	private int[] hitPoints = new int[3];
	private int curHitPoint = 0;


	public int curPathSegment = 0;
	// gameobject we're actually pointing at (may be useful for highlighting a target, etc.)
	private Collider _hitObject;
	public Collider hitObject { get { return _hitObject; } }
	

	void Start()
	{
		transform.Rotate (Vector3.right * 45.0f, Space.Self);
		//Debug.Log ("Start");

		radius = gameObject.GetComponent<SphereCollider>().radius;
	}

	void FixedUpdate()
	{
		if(!finished){

		if(path == null)
		{
			path = simulatePath();
		}
		else
		{
			animatePath();
		}
		}
	}

	void animatePath()
	{
		if(path == null){
			return;
		}

		if(curPathSegment >= path.Count)
		{
			transform.position = path[path.Count - 1];
			path = null;
			curPathSegment = 0;
			finished = true;
			Destroy (sightLine);
			return;
		}
		else if(curHitPoint >= 3){
			transform.position = path[path.Count - 1];
			path = null;
			curPathSegment = 0;
			finished = true;
			Destroy (sightLine);
			return;
		}
		transform.position = path[curPathSegment];
		var squareMagnitude = (transform.position - path[curPathSegment]).sqrMagnitude;
		if(curPathSegment >= path.Count)
		{
			transform.position = path[path.Count - 1];
			path = null;
			curPathSegment = 0;
			finished = true;
			Destroy (sightLine);
			return;
		}
		else if(Vector3.Distance(path[curPathSegment], path[hitPoints[curHitPoint]]) <= maxDistanceToGoal)
		{
			GameObject temp = Instantiate (ballSound, path[hitPoints[curHitPoint]], transform.rotation) as GameObject; //instantiate here at hitPoints[curHitPoint]
			temp.SendMessage ("initializeVariables", curHitPoint);
			curHitPoint++;
			float tempFloat = (float)ballSpeed;
			tempFloat = Mathf.Floor (tempFloat * 0.80f);
			ballSpeed = (int) tempFloat;
		}
		if(squareMagnitude < (maxDistanceToGoal * maxDistanceToGoal))
		{
			curPathSegment+= ballSpeed;
		}

	}
	
	/// <summary>
	/// Simulate the path of a launched ball.
	/// Slight errors are inherent in the numerical method used.
	/// </summary>
	public List<Vector3> simulatePath()
	{
		int bounceCount = 0;
		int j = 0;
		segmentCount = 0;
		//Debug.Log ("simulate path");
		List<Vector3> segments = new List<Vector3>();
		
		// The first line point is wherever the player's cannon, etc is
		segments.Add (transform.position);
		segmentCount++;
		j++;
		// The initial velocity
		Vector3 segVelocity = transform.up * playerFire.fireStrength * Time.deltaTime;
		
		// reset our hit object
		_hitObject = null;
		while(bounceCount < 3)
		{
			if(segmentCount > 7000){
				Debug.Log ("YOU FUCKED UP");
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

				hitPoints[bounceCount] = segmentCount;
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
		return segments;

	}
}