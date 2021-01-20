using UnityEngine;
using System.Collections;

public class SpikeTrap : MonoBehaviour {

	//How long to wait before next spike thrust
	public float delayTime;

	//Use this for initialization
	void Start () {
		StartCoroutine(ThrowSpikes ());
	}
	
	IEnumerator ThrowSpikes (){
		while (true){
			GetComponent<Animation>().Play();
			yield return new WaitForSeconds(delayTime);
		}
	}
}