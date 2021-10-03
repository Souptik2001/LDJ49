using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class MultiplayerGameManager : MonoBehaviour
{
    public GameObject mainMenuUI;
    public GameObject pauseMenuUI;
    public GameObject gameUI;
    public GameObject levelFragManager;
    public GameObject player;


    public bool gameIsPaused;
    public bool onMainMenu;
    static bool mainMenuInvoked;
    public TextMeshProUGUI corruptionTimeDisplay;
    ColorBlock prevSaveButtonColor;
    string redColorOpeningTag = "<#ff4800>";
    string greenColorOpeningTag = "<#1ae300>";
    string colorClosingTag = "</color>";


    public float score;


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
        score = 0;
        ResetScreenAndTimeEffects();
        gameIsPaused = false;
        onMainMenu = true;
        mainMenuInvoked = false;
        corruptionTimeDisplay.text = "" + score;
    }

    void Update()
    {
        corruptionTimeDisplay.text = "" + score;
        checkPause();
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
        Time.timeScale = 0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
        levelFragManager.SetActive(true);
        player.SetActive(true);
        gameUI.SetActive(true);
        mainMenuUI.SetActive(false);
        onMainMenu = false;
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
      prevTimeScale = Time.timeScale;
      Time.timeScale = 0f;
      pauseMenuUI.SetActive(true);
      gameIsPaused = true;
    }


    public void ExitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }


    public void PickUpPicked()
    {
        // Increase the score
        score++;
        // timeManager.DoSlowMotion();
    }


    public void Destabilize()
    {
        SaveSystem.DeleteSaveFiles();
        SceneManager.LoadScene("DemoScene");
    }


}
