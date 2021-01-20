using UnityEngine;
using System.Collections;

public class ChangeCamPosTrigger : MonoBehaviour {

	//pisition index for array in CameraLevelControl
	public int camPosIndex;

	public LevelCamControl camPosScript;

	void Start (){
		camPosScript = GameObject.Find("MainCamera").GetComponent<LevelCamControl>();
	}

	//trigger camera position change when player touches the trigger
	void OnTriggerStay (Collider other){
		if (other.tag == "Player"){
			camPosScript.ChangeCameraPosition(camPosIndex);
		}
	}
}
