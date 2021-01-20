using UnityEngine;
using System.Collections;

public class MouseMovement : MonoBehaviour {

	//Components
	private Camera cam;

	//Variables for raycast test
	private RaycastHit rayHit;
	private Ray ray;
	private bool hit;

	//Variables for intersected object
	private GameObject collideObject;
	private Vector3 objectPos;
	private float distance;
	private bool lockObject;

	//Called at the start of the code
	void Start (){
		cam = GetComponent<Camera>();
	}

	// Update is called once per frame
	void Update () {
		//If player presses the left mouse button 
		if(Input.GetButton("Left Mouse Button")){
			RaycastTest();
			MoveObject();
		} else {
			ResetCollider();
		}
	}

	//if raycast hits an object, check if it has a collider and asign it to a variable
	void RaycastTest (){
		ray = cam.ScreenPointToRay(Input.mousePosition);
		hit = Physics.Raycast(ray.origin,ray.direction,out rayHit);
		print(rayHit.collider.tag);
		if(hit && !lockObject){
			lockObject = true;
			collideObject = rayHit.collider.gameObject;
		}
	}

	//Move object if object is movable
	void MoveObject(){
		if(collideObject.tag == "Movable"){
			collideObject.GetComponent<Collider>().enabled = false;
			//Snap to grid if SnapModifier is pressed
			if(Input.GetButton("SnapModifier")){
				distance = rayHit.distance;
				print (collideObject.name);
				objectPos = ray.origin + distance*ray.direction;
				collideObject.transform.position = new Vector3 (Mathf.RoundToInt(objectPos.x),collideObject.transform.position.y,Mathf.RoundToInt(objectPos.z));
			} else {
				distance = rayHit.distance;
				print (collideObject.name);
				objectPos = ray.origin + distance*ray.direction;
				collideObject.transform.position = new Vector3 (objectPos.x,collideObject.transform.position.y,objectPos.z);
			}
		}
	}

	//Reset collider to work properly when released
	void ResetCollider(){
		lockObject = false;
		if(collideObject != null){
			collideObject.GetComponent<Collider>().enabled = true;
			collideObject = null;
		}
	}
}
