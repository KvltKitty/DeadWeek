using UnityEngine;
using System.Collections;

/// <summary>
/// Extend this class to add in any further data you want to be able to access
/// pertaining to an object the controller has collided with
/// </summary>
public class SuperCollisionType : MonoBehaviour {

    public float StandAngle = 80.0f; //maximum angle of slope the player can stand on without sliding
    public float SlopeLimit = 80.0f; //maximum slope the player can move upwards on
}
