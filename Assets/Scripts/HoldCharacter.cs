using UnityEngine;
using System.Collections;

public class HoldCharacter : MonoBehaviour {

	//Parrent colliding object so it rides along with the platform
	void OnTriggerEnter (Collider other){
		other.transform.parent = gameObject.transform;
	}

	//Unparrent colliding object when it leaves the platform 
	void OnTriggerExit (Collider other){
		other.transform.parent = null;
	}
}
