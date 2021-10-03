using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

    public float slowDownFactor = 0.05f;
    public float slowDownLength = 0.5f;
    public OverallGameManager overallGameManager;
    public MultiplayerGameManager multiplayerGameManager;

    void Start()
    {
        
    }


    void Update()
    {
        if (overallGameManager != null && overallGameManager.gameIsPaused) { return; }
        if(multiplayerGameManager!=null && multiplayerGameManager.gameIsPaused) { return; }
        Time.timeScale += (1f / slowDownLength) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }


    public void DoSlowMotion()
    {
        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

}
