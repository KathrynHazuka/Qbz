using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public bool promptsEnabled;

    //Set variables for score information
    private int currentScore;
    private int highScore;

    //Coin system
    [HideInInspector]
    public int coinCount;
    private int totalCoinCount;

    //Level information
    private int totalLevelCount = 2;

    private int currentLevel;
    private int continueOnLevel;
    private bool showWinScreen = false;
    public bool levelCompleted = false;

    [SerializeField]
    private float currentTime;
    public bool countTime = true;

    //References
    private GameObject coinParent;

    //Player information
    [SerializeField]
    private GameObject playerGameObject;
    private Transform spawn;

    private UIManager uiManager;

    //Runs at start
    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();

        //Get reference to coin parent
        coinParent = GameObject.Find("_Map/_Level/_Coins");
        //Check how many coins there are in a level
        totalCoinCount = coinParent.transform.childCount;

        //If there is saved data on local computer, set what level they should be on
        if (PlayerPrefs.GetInt("Level Completed") > 0)
            continueOnLevel = PlayerPrefs.GetInt("Level Completed");
        else
            //If no saved data is found, set the next level to be played as Level 1
            continueOnLevel = 1;

        spawn = GameObject.FindGameObjectWithTag("Spawn").transform;
        Instantiate(playerGameObject, spawn.position, spawn.rotation);

        //Turn on player functions that use game manager
        playerGameObject.GetComponent<Player>().usesManager = true;
    }

    //Runs once per frame update
    void Update()
    {
        TimeManager();
        uiManager.UpdateTimerAndCoins(currentTime, coinCount, totalCoinCount);
    }

    void TimeManager()
    {
        //If player has not completes level yet, decrease timer, set current timer to a formatted string
        if (!levelCompleted && countTime)
        {
            currentTime -= Time.deltaTime;
            //If time runs out before level is completed, reset timer and draw out of time GUI
            if (currentTime <= 0)
            {
                //Reset timer and TODO: draw out of time GUI
                currentTime = 0;
                print("Time Ran out!");
            }
        }
    }

    //Runs once player reaches end of level
    public void CompleteLevel()
    {
        levelCompleted = true;
        showWinScreen = true;

        uiManager.OpenLevelComplMenu(currentTime, coinCount, totalCoinCount, currentLevel);

        //If current level is the level you should continue on, set the continueOnLevel to next level
        if (currentLevel == continueOnLevel)
        {
            continueOnLevel += 1;
        }
        SaveGame();
    }

    //Load next level, depending on what level you are on
    public void LoadNextLevel()
    {
        //If current level is not the last level, load next level
        if (currentLevel != totalLevelCount)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Level " + (currentLevel + 1).ToString());
            // needed? //SaveGame();
            // needed? //PlayerPrefs.GetInt ("Level Completed");
        }
        else if (currentLevel == totalLevelCount)
        {
            print("You win!");
        }
    }

    //Save values of the next level to complete and current score
    void SaveGame()
    {
        PlayerPrefs.SetInt("Level Completed", continueOnLevel);
        print(continueOnLevel);
        PlayerPrefs.SetInt("Level" + continueOnLevel.ToString() + "score", currentScore);
    }
}