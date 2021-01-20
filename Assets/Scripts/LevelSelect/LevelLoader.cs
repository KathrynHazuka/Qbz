using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour {

	//Set gameobject for lock that shows on locked levels
	public GameObject padLock;

	//Set what level this cube should load
	public int levelToLoad;

	//
	private int completedLevel;

	//String to display prompts
	private string loadPrompt;
	private bool inRange = false;
	private bool canLoadLevel;

	//Load level infromation and lock level if neccessary
	void Start (){
		completedLevel = PlayerPrefs.GetInt("Level Completed");
		LockLevel();
	}

	//Update is called every frame
	void Update (){
		//If player presses the action button, load level
		if (canLoadLevel && inRange && Input.GetButtonDown("Action1")){
			Application.LoadLevel("Level " + levelToLoad.ToString());
		}
	}

	//Create a lock if level should be locked
	void LockLevel(){
		canLoadLevel = levelToLoad <= completedLevel ? true : false;
		if (!canLoadLevel){
			Instantiate (padLock, new Vector3(transform.position.x, 0f, transform.position.z - 0.62f), Quaternion.Euler(0,90,0));
		}
	}

	//Run when touching a trigger
	void OnTriggerStay (Collider other){
		inRange = true;
		//Create string for prompts
		if (canLoadLevel){
			loadPrompt = "[E] to load level " + levelToLoad.ToString();
		} else {
			loadPrompt = "Level " + levelToLoad.ToString() + " is locked";
		}
	}

	//Delete string for prompts when player moves away
	void OnTriggerExit (){
		loadPrompt = "";
		inRange = false;
	}

	//Draw GUI
	void OnGUI (){
		//Draw prompt with previously set string
		GUI.Label (new Rect(30, Screen.height*.9f,200,40), loadPrompt);
	}
}
