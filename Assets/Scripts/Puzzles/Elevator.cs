using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour {

	//elevator state variables
	public string elevatorState;
	public float timeOnTop;

	//Components
	private Animation anim;


	//Assign Components
	void Start(){
		anim = GetComponent<Animation>();
	}

	//Update is called once per frame
	void Update(){
		//check if there is no animations playing
		if(!anim.isPlaying){
			SetElevatorState();
		}
	}

	void OnTriggerEnter(Collider other){
		//Move elevator if player hits the trigger
		if(other.tag == "Player"){
			MoveElevator();
		}
	}

	//Set state of elevator
	void SetElevatorState(){
		//if position of elevator is on top, set state to "On Top"
		if (Mathf.RoundToInt(transform.position.y) == 1){
			elevatorState = "On Top";
			timeOnTop += Time.deltaTime;
		}
		
		//if position of elevator is on bottom, set state to "On Bottom"
		if (Mathf.RoundToInt(transform.position.y) == 0){
			elevatorState = "On Bottom";
		}
		
		//move alavator down when it is in "On Top" state for 2 seconds
		if (timeOnTop >= 2f){
			anim.Play("ElevatorDownAnim");
			timeOnTop = 0f;
		}
	}

	//Move elevator according to state
	void MoveElevator(){
		//Move elevator up
		if (elevatorState == "On Bottom"){
			if (!anim.isPlaying){
				anim.Play("ElevatorUpAnim");
			}
		}

		//Move elevator down
		if (elevatorState == "On Top"){
			if (!anim.isPlaying){
				anim.Play("ElevatorDownAnim");
			}
		}
	}
}
