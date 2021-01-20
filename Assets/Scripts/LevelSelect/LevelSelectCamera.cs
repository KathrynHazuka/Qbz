using UnityEngine;
using System.Collections;

public class CameraControls : MonoBehaviour {

	public GameObject player;

	// Update is called once per frame
	void Update () {
		//Make camera look at the player
		transform.LookAt (player.transform);
	}
}
