using UnityEngine;
using System.Collections;

public class Partol : MonoBehaviour {

	public float moveSpeed;

	//array for patrol points
	public Transform[] patrolPoints;

	//array index number for current point
	private int currentPoint;

	// Use this for initialization
	void Start () {
		transform.position = patrolPoints[0].position;
		currentPoint = 0;
	}
	
	// Update is called once per frame
	void Update () {

		//checking if i am at the target location
		if (transform.position == patrolPoints[currentPoint].position){
			//Inctrement array index number for current position
			currentPoint++;
		}

		//Reset array index number when end of array is reached
		if (currentPoint >= patrolPoints.Length){
			currentPoint = 0;
		}

		//Move object towords new location
		transform.position = Vector3.MoveTowards(transform.position, patrolPoints[currentPoint].position, moveSpeed * Time.deltaTime); 

	}
}
