using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class OverallGameManager : MonoBehaviour
{
    public bool pauseMenuUnlocked;
    public bool saveSystemFound;
    public bool pauseMenuRectified;
    // When this pauseMenuRectified is found the save system will be corrupted
    public bool multiplayerSystemFound;
    float corruptionTime;
    float corruptionTimeNumber;
    public GameObject mainMenuUI;
    public GameObject pauseMenuUI;
    public GameObject gameUI;
    public GameObject levelFragManager;
    public GameObject player;


    public bool gameIsPaused;
    public bool onMainMenu;
    static bool mainMenuInvoked;
    public TextMeshProUGUI corruptionTimeDisplay;
    public TextMeshProUGUI saveSystemText;
    public TextMeshProUGUI checkListText;
    public TextMeshProUGUI manualSaveInstruction;
    public Button saveGameButton;
    public GameObject alertPanel;
    ColorBlock prevSaveButtonColor;
    string redColorOpeningTag = "<#ff4800>";
    string greenColorOpeningTag = "<#1ae300>";
    string colorClosingTag = "</color>";



    // Animation
    public Animator saveGameTextAnimator;
    private string currentSaveGameTextAnimation;


    // Time slow down // To slow down time just set decrease speed to true and set a value for targetTimeScale
    float prevTimeScale;
    public TimeManager timeManager;


    void ResetScreenAndTimeEffects()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        prevTimeScale = 1f;
    }


    void Start()
    {
        ResetScreenAndTimeEffects();
        currentSaveGameTextAnimation = null;
        prevSaveButtonColor = saveGameButton.colors;
        CheckForSaveFile();
        onMainMenu = true;
        UpdateCheckListDisplay();
        gameIsPaused = false;
        mainMenuInvoked = false;
        corruptionTime = 30f;
        corruptionTimeNumber = corruptionTime;
        corruptionTimeDisplay.text = "" + corruptionTimeNumber;
    }

    void Update()
    {
        if(!onMainMenu && !mainMenuInvoked)
        {
            Invoke("GetToMainMenu", corruptionTime);
            mainMenuInvoked = true;
        }
        if (!onMainMenu && pauseMenuUnlocked)
        {
            checkPause();
        }
    }

    void DecreaseCorruptionTime()
    {
        corruptionTimeNumber--;
        if (corruptionTimeNumber < 0) { corruptionTimeNumber = 0; }
        corruptionTimeDisplay.text = "" + corruptionTimeNumber;
        if (!onMainMenu)
        {
            Invoke("DecreaseCorruptionTime", 1f);
        }
    }

    void UpdateCheckListDisplay()
    {
        string displayString = "";
        if (pauseMenuUnlocked == true)
        {
            displayString += greenColorOpeningTag;
        }
        else
        {
            displayString += redColorOpeningTag;
        }
        displayString += "> Buggy Pause Menu Unlocked";
        displayString += colorClosingTag+"\n";
        if (saveSystemFound == true)
        {
            displayString += greenColorOpeningTag;
        }
        else
        {
            displayString += redColorOpeningTag;
        }
        displayString += "> Save System Found";
        displayString += colorClosingTag+"\n";
        if (pauseMenuRectified == true && multiplayerSystemFound==false)
        {
            if (!onMainMenu)
            {
                FindObjectOfType<AudioManager>().Play("AlertDescription");
                FindObjectOfType<AudioManager>().Play("Alarm");
            }
            manualSaveInstruction.text = "> Save system has been corrupted! Save file will be deleted very soon! Copy the save file named 'saveData' from the path : " + Application.persistentDataPath + " and keep that file some where else. Before starting to play place that save file at the above mentioned path.";
            alertPanel.SetActive(true);
            prevSaveButtonColor = saveGameButton.colors;
            ColorBlock colors = saveGameButton.colors;
            colors.normalColor = Color.red;
            colors.highlightedColor = Color.red;
            saveGameButton.colors = colors;
            displayString += greenColorOpeningTag;
        }
        else
        {
            alertPanel.SetActive(false);
            saveGameButton.colors = prevSaveButtonColor;
            displayString += redColorOpeningTag;
        }
        displayString += "> Pause Menu Rectified";
        displayString += colorClosingTag+"\n";
        if (multiplayerSystemFound == true)
        {
            saveGameButton.colors = prevSaveButtonColor;
            displayString += greenColorOpeningTag;
        }
        else
        {
            displayString += redColorOpeningTag;
        }
        displayString += "> Connected To Server";
        displayString += colorClosingTag+"\n";
        checkListText.text = displayString;
    }


    void ResetEverything()
    {
        pauseMenuUnlocked = false;
        saveSystemFound = false;
        pauseMenuRectified = false;
        multiplayerSystemFound = false;
    }

    void CheckForSaveFile()
    {
        PlayerData saveData = SaveSystem.LoadData();
        if (saveData == null)
        {
            pauseMenuUnlocked = false;
            saveSystemFound = false;
            pauseMenuRectified = false;
            multiplayerSystemFound = false;
        }
        else
        {
            bool falseChecker = true;
            pauseMenuUnlocked = saveData.pauseMenuUnlocked;
            saveSystemFound = saveData.saveSystemFound;
            pauseMenuRectified = saveData.pauseMenuRectified;
            multiplayerSystemFound = saveData.multiplayerSystemFound;
            falseChecker = falseChecker && pauseMenuUnlocked;
            falseChecker = falseChecker && saveSystemFound;
            falseChecker = falseChecker && pauseMenuRectified;
            falseChecker = falseChecker && multiplayerSystemFound;
            if (falseChecker == true)
            {
                SceneManager.LoadScene("MultiplayerScene");
            }
        }
    }


    //void GetToMainMenu()
    //{
    //    if (pauseMenuRectified == true && multiplayerSystemFound == false)
    //    {
    //        SaveSystem.DeleteSaveFiles();
    //    }
    //    ResetEverything();
    //    onMainMenu = true;
    //    mainMenuUI.SetActive(true);
    //    pauseMenuUI.SetActive(false);
    //    gameIsPaused = false;
    //    Time.timeScale = 1f;
    //    gameUI.SetActive(false);
    //    levelFragManager.SetActive(false);
    //    player.SetActive(false);
    //}

    public void GetToMainMenu()
    {
        FindObjectOfType<AudioManager>().Stop("SaveDescription");
        FindObjectOfType<AudioManager>().Stop("AlertDescription");
        FindObjectOfType<AudioManager>().Stop("Alarm");
        if (pauseMenuRectified == true && multiplayerSystemFound == false)
        {
            SaveSystem.DeleteSaveFiles();
        }
        Time.timeScale = 0f;
        ResetEverything();
        SceneManager.LoadScene( SceneManager.GetActiveScene().name);
        //ResetEverything();
        //onMainMenu = true;
        //mainMenuUI.SetActive(true);
        //pauseMenuUI.SetActive(false);
        //gameIsPaused = false;
        //Time.timeScale = 1f;
        //gameUI.SetActive(false);
        //levelFragManager.SetActive(false);
        //player.SetActive(false);
    }


    public void PlayGame()
    {
        CheckForSaveFile();
        onMainMenu = false;
        UpdateCheckListDisplay();
        levelFragManager.SetActive(true);
        player.SetActive(true);
        gameUI.SetActive(true);
        mainMenuUI.SetActive(false);
        corruptionTime = 30f;
        corruptionTimeNumber = corruptionTime;
        Invoke("DecreaseCorruptionTime", 1f);
        mainMenuInvoked = false;
    }


    void checkPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !onMainMenu)
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        Time.timeScale = prevTimeScale;
        pauseMenuUI.SetActive(false);
        gameIsPaused = false;
    }

    void Pause()
    {
        if (pauseMenuRectified)
        {
            prevTimeScale = Time.timeScale;
            Time.timeScale = 0f;
        }
        pauseMenuUI.SetActive(true);
        gameIsPaused = true;
    }


    public void SaveGame(bool autosave)
    {
        if (autosave || (saveSystemFound && !pauseMenuRectified))
        {
            SaveSystem.SaveData(this);
            // Show visual feedback
            if (autosave)
            {
                saveSystemText.text = "<#00CA02>Fallback Command : Autosave...";
            }
            else
            {
                saveSystemText.text = "<#00CA02>Components Saved";
            }
            ChangeAnimationState(saveGameTextAnimator, ref currentSaveGameTextAnimation, "gameSaveUI");
        }
        else
        {
            // show visual feedback
            if (saveSystemFound)
            {
                saveSystemText.text = "<#EB0800>Save system corrupted !";
            }
            else
            {
                saveSystemText.text = "<#EB0800>Save system not present !";
            }
            ChangeAnimationState(saveGameTextAnimator, ref currentSaveGameTextAnimation, "gameSaveUI");
        }
    }


    public void ExitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }


    public void PickUpPicked()
    {
        if (pauseMenuUnlocked == false)
        {
            pauseMenuUnlocked = true;
        }else if(saveSystemFound == false)
        {
            FindObjectOfType<AudioManager>().Play("SaveDescription");
            saveSystemFound = true;
        }else if(pauseMenuRectified == false)
        {
            FindObjectOfType<AudioManager>().Stop("SaveDescription");
            pauseMenuRectified = true;
            SaveGame(true);
            timeManager.slowDownLength = 20f;
            timeManager.slowDownFactor = 0.005f;
        }
        else if(multiplayerSystemFound == false)
        {
            multiplayerSystemFound = true;
            FindObjectOfType<AudioManager>().Stop("SaveDescription");
            FindObjectOfType<AudioManager>().Stop("AlertDescription");
            FindObjectOfType<AudioManager>().Stop("Alarm");
            SaveGame(true);
            SceneManager.LoadScene("MultiplayerScene");
            // Redirect it to multiplayer scene
        }
        UpdateCheckListDisplay();
        timeManager.DoSlowMotion();
    }




    public void ChangeAnimationState(Animator animator, ref string currentAnimationName, string animationName)
    {
        if (currentAnimationName!=null && currentAnimationName == animationName) return;
        animator.Play(animationName);
        currentAnimationName = animationName;
        Invoke("RevokeAnimationName", animator.GetCurrentAnimatorStateInfo(0).length);
    }


    public void RevokeAnimationName()
    {
        saveGameTextAnimator.Play("gameSaveUIIdle");
        currentSaveGameTextAnimation = null;
    }


    public void GetHelp()
    {
        SceneManager.LoadScene("DemoScene");
    }



}
