using UnityEngine;

public class PlayerFire : MonoBehaviour
{
	public float fireStrength = 400;
	public Color nextColor = Color.red;
	public float tossAngle;

	public void setFireStrength(float strength)
	{
		fireStrength = Mathf.Floor(strength);
		//Debug.Log ("Set strength");
	}
	public void setTossAngle(float variable)
	{
		tossAngle = 40.0f + variable;
	}
}