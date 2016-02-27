using UnityEngine;
using System.Collections;

public class MansionQuickDirtyReturn : MonoBehaviour {
	public GameObject lobbyReturn;
	public GameObject player;
	void returnToLobby(){
		if(player != null)
		{
			player.transform.position = lobbyReturn.transform.position;
		}
	}

}
