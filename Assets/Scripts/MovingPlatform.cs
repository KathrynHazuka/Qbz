using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {

	//Playtofm and movement point infromation
	public Transform movingPlatform;
	public Transform position1;
	public Transform position2;

	//New position the platform will move to
	public Vector3 newPosition;

	//State of the platform
	public string CurrentState;

	//How long it takes to move
	public float smooth;

	//How often to change to next target
	public float resetTime;

	void Start (){
		ChangeTarget();
	}

	//Move platform between positions
	void FixedUpdate (){
		movingPlatform.position = Vector3.Lerp (movingPlatform.position, newPosition, smooth * Time.deltaTime);
	}

	//Change target based on state
	void ChangeTarget (){
		if(CurrentState == "Moving to position 1"){
			CurrentState = "Moving to position 2";
			newPosition = position2.position;
		} 
		else if (CurrentState == "Moving to position 2"){
			CurrentState = "Moving to position 1";
			newPosition = position1.position;
		} 
		else if (CurrentState == ""){
			CurrentState = "Moving to position 2";
			newPosition = position2.position;
		}
		//Rerun ChanceTarget every "resetTime" seconds
		Invoke("ChangeTarget", resetTime);
	}
}
