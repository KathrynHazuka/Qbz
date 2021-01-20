using UnityEngine;
using System.Collections;

public class LevelCamControl : MonoBehaviour {

	public Transform[] camPositions;
	public float smooth = 5;
	public Vector3 nextCamPos;

	//Set initial camera position
	void Start () {
		//If there are predetermened camera positions in the world, move the camera to the first position
		if (camPositions.Length != 0){
			transform.position = camPositions[0].transform.position;
			transform.rotation = camPositions[0].transform.rotation;
			nextCamPos = camPositions[0].position;
		}
	}

	//Change nextCamPos to next position the camera will move to
	public void ChangeCameraPosition (int camPosIndex){
		nextCamPos = camPositions[camPosIndex].position;
	}

	//Gradually move the camera to new position
	void FixedUpdate (){
		//If there are predetermened camera positions in the world, move the camera to the next position when triggered
		if(camPositions.Length != 0){
			transform.position = Vector3.Lerp (transform.position, nextCamPos, smooth * Time.deltaTime);
		}
	}
}