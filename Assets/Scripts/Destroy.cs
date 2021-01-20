using UnityEngine;
using System.Collections;

public class Destroy : MonoBehaviour {

	public float lifeTime = 0;

	//Destroy object within specified time
	void Start () {
		Destroy(gameObject, lifeTime);
	}
}
