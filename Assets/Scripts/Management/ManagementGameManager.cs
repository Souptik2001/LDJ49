using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ManagementGameManager : MonoBehaviour
{
    void Start()
    {
        CheckForSaveFile();
    }

    void CheckForSaveFile()
    {
        PlayerData saveData = SaveSystem.LoadData();
        if (saveData == null)
        {
            Invoke("LoadPrototypeScene", 5f);
        }
        else
        {
            bool falseChecker = true;

            falseChecker = falseChecker && saveData.pauseMenuUnlocked;
            falseChecker = falseChecker && saveData.saveSystemFound;
            falseChecker = falseChecker && saveData.pauseMenuRectified;
            falseChecker = falseChecker && saveData.multiplayerSystemFound;
            if (falseChecker == true)
            {
                Invoke("LoadMultiplayerScene", 5f);
            }
            else
            {
                Invoke("LoadPrototypeScene", 5f);
            }
        }
    }


    void LoadPrototypeScene()
    {
        SceneManager.LoadScene("DemoScene");
    }

    void LoadMultiplayerScene()
    {
        SceneManager.LoadScene("MultiplayerScene");
    }






}
