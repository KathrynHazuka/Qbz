using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    private MenuInfo gameInfoMenu;
    private Text timeText;
    private Text timerText;
    private Text coinsText;

    private int previousCoinCount = -1;
    private float previousTime = -1;

    private Color warningColor = new Color(233 / 255f, 87 / 255f, 87 / 255f, 1);
    private Color defaultColor = Color.white;

    private GameObject gameInfoPanel;

    private MenuInfo pauseMenu;

    private MenuInfo levelComplMenu;
    private Text congratsText;
    private Text endTimeText;
    private Text endCoinsText;

    private MenuInfo confirmationMenu;
    private Button confirmationButton;
    private bool reopenConfirmationMenu;

    private Text promptText;

    private GameManager gameManager;

    // Use this for initialization
    void Start ()
    {
        InitializeGameInfoPanel();
        InitializePauseMenuInfo();
        InitializelevelComplMenuInfo();
        InitializeConfirmationMenuInfo();

        promptText = GameObject.Find("PromptText").GetComponent<Text>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void InitializeGameInfoPanel()
    {
        gameInfoPanel = GameObject.Find("GameInfoPanel");

        timeText = GameObject.Find("Time:").GetComponent<Text>();
        timerText = GameObject.Find("TimerText").GetComponent<Text>();
        coinsText = GameObject.Find("CoinsText").GetComponent<Text>();
    }

    void InitializePauseMenuInfo()
    {
        pauseMenu = new MenuInfo(GameObject.Find("PauseMenuPanel"),
            GameObject.Find("PauseMenuPanel").GetComponent<RectTransform>(),
            Quaternion.Euler(new Vector3(0, 80.3f, 0)),
            Quaternion.Euler(Vector3.zero),
            0.05f);

        pauseMenu.gameObject.SetActive(false);
    }

    void InitializelevelComplMenuInfo()
    {
        congratsText = GameObject.Find("CongratsText").GetComponent<Text>();
        endTimeText = GameObject.Find("EndTimeText").GetComponent<Text>();
        endCoinsText = GameObject.Find("EndCoinsText").GetComponent<Text>();

        levelComplMenu = new MenuInfo(GameObject.Find("LevelCompletePanel"),
            GameObject.Find("LevelCompletePanel").GetComponent<RectTransform>(),
            Quaternion.Euler(new Vector3(77.6f, 0, 0)),
            Quaternion.Euler(Vector3.zero),
            0.025f);

        levelComplMenu.gameObject.SetActive(false);
    }

    void InitializeConfirmationMenuInfo()
    {
        reopenConfirmationMenu = false;
        confirmationButton = GameObject.Find("YesButton").GetComponent<Button>();

        confirmationMenu = new MenuInfo(GameObject.Find("ConfirmationPanel"),
            GameObject.Find("ConfirmationPanel").GetComponent<RectTransform>(),
            Quaternion.Euler(new Vector3(102.7f, 0, 0)),
            Quaternion.Euler(Vector3.zero),
            0.05f);

        confirmationMenu.gameObject.SetActive(false);
    }

    public void UpdateTimerAndCoins(float currentTime, int coinCount, int totalCoinCount)
    {
        //If timer gets below 5 seconds, change text color to red
        if (currentTime <= 30f)
        {
            if (timerText.color != warningColor)
            {
                timerText.color = warningColor;
                timeText.color = warningColor;
            }
        }
        else
        {
            if (timerText.color != defaultColor)
            {
                timerText.color = defaultColor;
                timeText.color = defaultColor;
            }
        }

        if (currentTime != previousTime)
        {
            string currentFormatTime = string.Format("{0:0.0}", currentTime);
            timerText.text = currentFormatTime;
        }
        previousTime = currentTime;

        if (coinCount != previousCoinCount)
        {
            coinsText.text = coinCount + "/" + totalCoinCount;
        }
        previousCoinCount = coinCount;
    }

    public void SetPromptText(string prompt)
    {
        promptText.text = prompt;
    }

    void Update()
    {
        CheckInputs();

        PauseMenuRotation();
        LevelComplMenuRotation();
        ConfirmationMenuRotation();

        ReopenConfirmationMenu();
    }

    void CheckInputs()
    {
        if (Input.GetButtonDown("Open Pause Menu") && !gameManager.levelCompleted)
            TogglePauseMenu();
    }

    //-------------------------------------------------------Pause Menu Functions----------------------------------------------------------

    public void TogglePauseMenu()
    {
        confirmationMenu.open = false;
        reopenConfirmationMenu = false;

        if (!pauseMenu.open)
        {
            Time.timeScale = 0;
            pauseMenu.open = true;
            gameManager.countTime = false;
            FindObjectOfType<Player>().canMove = false;
        }
        else
        {
            pauseMenu.open = false;
            gameManager.countTime = true;
            FindObjectOfType<Player>().canMove = true;
        }
    }

    void PauseMenuRotation()
    {
        if (pauseMenu.open)
        {
            if (pauseMenu.openTimer < 1)
                pauseMenu.openTimer += pauseMenu.openSpeed;

            pauseMenu.rectTransform.localRotation = Quaternion.Slerp(pauseMenu.closedRotation, pauseMenu.openRotation, pauseMenu.openTimer);

            pauseMenu.gameObject.SetActive(true);

            if (pauseMenu.closeTimer != 0)
                pauseMenu.closeTimer = 0;
        }
        else
        {
            if (confirmationMenu.rectTransform.localRotation != confirmationMenu.closedRotation)
                return;

            if (pauseMenu.closeTimer < 1)
                pauseMenu.closeTimer += pauseMenu.openSpeed;

            pauseMenu.rectTransform.localRotation = Quaternion.Slerp(pauseMenu.openRotation, pauseMenu.closedRotation, pauseMenu.closeTimer);

            if (pauseMenu.rectTransform.localRotation == pauseMenu.closedRotation)
                Time.timeScale = 1;

            if (pauseMenu.rectTransform.localRotation == pauseMenu.closedRotation)
                pauseMenu.gameObject.SetActive(false);

            if (pauseMenu.openTimer != 0)
                pauseMenu.openTimer = 0;
        }
    }

    //-----------------------------------------------------Options Menu Functions--------------------------------------------------------

    public void OpenOptionsMenu()
    {
        confirmationMenu.open = false;
        reopenConfirmationMenu = false;
    }

    //-------------------------------------------------Level Complete Menu Functions-----------------------------------------------------

    public void OpenLevelComplMenu(float currentTime, int coinCount, int totalCoinCount, int currentLevel)
    {
        congratsText.text = string.Format(congratsText.text, currentLevel);

        string currentFormatTime = string.Format("{0:0.0}", currentTime);
        endTimeText.text = string.Format(endTimeText.text, currentFormatTime);

        endCoinsText.text = string.Format(endCoinsText.text, coinCount, totalCoinCount);

        if (!levelComplMenu.open)
        {
            Time.timeScale = 0;
            gameInfoPanel.SetActive(false);
            levelComplMenu.open = true;
            FindObjectOfType<Player>().canMove = false;
        }
        else
        {
            Time.timeScale = 1;
            gameInfoPanel.SetActive(true);
            levelComplMenu.open = false;
        }
    }

    void LevelComplMenuRotation()
    {
        if (levelComplMenu.open)
        {
            if (levelComplMenu.openTimer < 1)
                levelComplMenu.openTimer += levelComplMenu.openSpeed;

            levelComplMenu.rectTransform.localRotation = Quaternion.Slerp(levelComplMenu.closedRotation, levelComplMenu.openRotation, levelComplMenu.openTimer);

            levelComplMenu.gameObject.SetActive(true);

            if (levelComplMenu.closeTimer != 0)
                levelComplMenu.closeTimer = 0;
        }
        else
        {
            if (levelComplMenu.closeTimer < 1)
                levelComplMenu.closeTimer += levelComplMenu.openSpeed;

            levelComplMenu.rectTransform.localRotation = Quaternion.Slerp(levelComplMenu.openRotation, levelComplMenu.closedRotation, levelComplMenu.closeTimer);

            if (levelComplMenu.rectTransform.localRotation == levelComplMenu.closedRotation)
                levelComplMenu.gameObject.SetActive(false);

            if (levelComplMenu.openTimer != 0)
                levelComplMenu.openTimer = 0;
        }
    }

    public void LoadNextLevel()
    {
        confirmationMenu.open = false;
        reopenConfirmationMenu = false;
        gameManager.LoadNextLevel();
    }

    //-------------------------------------------------Confirmation Menu Functions-----------------------------------------------------

    public void ConfirmLevelSelectLoad()
    {
        confirmationButton.onClick.RemoveAllListeners();
        confirmationButton.onClick.AddListener(LoadLevelSelect);
        OpenConfirmationMenu();
    }

    public void ConfirmMainMenuLoad()
    {
        confirmationButton.onClick.RemoveAllListeners();
        confirmationButton.onClick.AddListener(LoadMainMenu);
        OpenConfirmationMenu();
    }

    public void ConfirmQuit()
    {
        confirmationButton.onClick.RemoveAllListeners();
        confirmationButton.onClick.AddListener(Quit);
        OpenConfirmationMenu();
    }

    void OpenConfirmationMenu()
    {
        if (confirmationMenu.open)
        {
            confirmationMenu.open = false;
            reopenConfirmationMenu = true;
        }
        else
        {
            confirmationMenu.open = true;
        }
    }

    void ReopenConfirmationMenu()
    {
        if (reopenConfirmationMenu && confirmationMenu.rectTransform.localRotation == confirmationMenu.closedRotation)
        {
            confirmationMenu.open = true;
        }
        else if (reopenConfirmationMenu && confirmationMenu.rectTransform.localRotation == confirmationMenu.openRotation)
        {
            reopenConfirmationMenu = false;
        }
    }

    public void CloseConfirmationMenu()
    {
        confirmationMenu.open = false;
        reopenConfirmationMenu = false;
    }

    void ConfirmationMenuRotation()
    {
        if (confirmationMenu.open)
        {
            if (confirmationMenu.openTimer < 1)
                confirmationMenu.openTimer += confirmationMenu.openSpeed;

            confirmationMenu.rectTransform.localRotation = Quaternion.Slerp(confirmationMenu.closedRotation, confirmationMenu.openRotation, confirmationMenu.openTimer);

            confirmationMenu.gameObject.SetActive(true);

            if (confirmationMenu.closeTimer != 0)
                confirmationMenu.closeTimer = 0;
        }
        else
        {
            if (confirmationMenu.closeTimer < 1)
                confirmationMenu.closeTimer += confirmationMenu.openSpeed;

            confirmationMenu.rectTransform.localRotation = Quaternion.Slerp(confirmationMenu.openRotation, confirmationMenu.closedRotation, confirmationMenu.closeTimer);

            if (confirmationMenu.rectTransform.localRotation == confirmationMenu.closedRotation)
                confirmationMenu.gameObject.SetActive(false);

            if (confirmationMenu.openTimer != 0)
                confirmationMenu.openTimer = 0;
        }
    }

    void LoadLevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void Quit()
    {
        Application.Quit();
    }
}

struct MenuInfo
{
    public GameObject gameObject;
    public RectTransform rectTransform;
    public Quaternion openRotation;
    public Quaternion closedRotation;
    public float openTimer;
    public float closeTimer;
    public float openSpeed;
    public bool open;

    public MenuInfo(GameObject _gameObject, RectTransform _rectTransform, Quaternion _closedRotation, Quaternion _openRotation, float _openSpeed)
    {
        gameObject = _gameObject;
        rectTransform = _rectTransform;
        closedRotation = _closedRotation;
        openRotation = _openRotation;
        openTimer = 0;
        closeTimer = 0;
        openSpeed = _openSpeed;
        open = false;
    }
}

