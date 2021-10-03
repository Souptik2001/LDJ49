using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class PlayerData
{
    public bool pauseMenuUnlocked;
    public bool saveSystemFound;
    public bool pauseMenuRectified;
    // When this pauseMenuRectified is found the save system will be corrupted
    public bool multiplayerSystemFound;


    public PlayerData(OverallGameManager gameManager)
    {
        pauseMenuUnlocked = gameManager.pauseMenuUnlocked;
        pauseMenuRectified = gameManager.pauseMenuRectified;
        saveSystemFound = gameManager.saveSystemFound;
        multiplayerSystemFound = gameManager.multiplayerSystemFound;
    }

}
