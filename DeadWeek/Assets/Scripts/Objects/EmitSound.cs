using UnityEngine;
using System.Collections;

//Script for an object to emit a sound at its location and have all objects in range react to it
//To use attach this to an object that has a noise on it. When that noise is played, run the CheckForObjects function.

public class EmitSound : MonoBehaviour {


    public GameObject guard;
    public GameObject monster;
    public GameObject enemy;
    public int loudness = 50;

	private bool soundPlaying;
	void Start(){
		soundPlaying = false;
	}

    void Update()
    {
        if (soundPlaying)
		{
            CheckForbjects();
		}
    }


    public void CheckForbjects()
    {
        //Create an array of all objects in range of the sound. Loudness is the range. adjust to differ how far the sound travels
        Collider[] Enemies = Physics.OverlapSphere(this.transform.position, loudness); 
        int enemyCount = 0;
        while (enemyCount < Enemies.Length)
        {
            //for every guard in range (half of the loudness since they cant hear as far) tell them to investigate sound
            if (Enemies[enemyCount].tag == ("Guard") && (Vector3.Distance(transform.position,Enemies[enemyCount].transform.position) <= loudness/2))
            {
                enemy = Enemies[enemyCount].gameObject;
                //AI_Main AI_Main_Script = enemy.GetComponent<AI_Main>();
                //AI_Main_Script.NoiseHeard(this.gameObject);
            }
            //for every monster in range tell them to investigate sound
            if (Enemies[enemyCount].tag == ("Monster"))
            {
                enemy = Enemies[enemyCount].gameObject;
                //AI_Main AI_Main_Script = enemy.GetComponent<AI_Main>();
                //AI_Main_Script.NoiseHeard(this.gameObject);
            }
            enemyCount++;
        }
    }

	public void toggleSoundPlaying(){
		soundPlaying = !soundPlaying;
		Debug.Log (soundPlaying);
	}

	void OnTriggerEnter(Collider other){
		Debug.Log ("HIT");
	}
}
