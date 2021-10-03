using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoManager : MonoBehaviour
{
    string input;
    public TypeWritterEffect twEffect;
    void Start()
    {
        input = ">> Hello game enthusiasts,\n>> This is my submission for Ludum Dare 49, or should I say it was supposed to be.Due to a shortage of time, I was not able to develop my game properly and it became a buggy, UNSTABLE game.\n>> It was supposed to be an endless runner type of game.But alas that was not completed.\n>> But still, there is hope.You can help me to bring my game to a form.So, sorry to say but you have to take a journey through my unstable game and hopefully you will succeed to establish stability in my game. Don’t worry I will guide you through the steps.\n>> So, I will let you in a prototype version of the game. That will be similar to the original game but you have to find the pieces of the game components.\n>> You will get a total of 4 components.\n>> The first one is the pause menu component but I am sorry that will be a buggy pause menu and will not pause the game if you press escape.\n>> The second one is a save system component. Using which you can save your game progress.\n>> The third one is the rectified pause menu.\n>> And the last and final one is a server connection component that will enable you to play the endless runner game.\n>> But the main unstableness is that the game will shut down after every 15 seconds or so.And if you have not found the save system till that time and didn’t save the game then you will lose all the components and have to start collecting the components from the beginning.\n>> So, sorry for this inconvenience and also good luck with the journey.\n>> Loading Prototype........";
        FindObjectOfType<AudioManager>().Play("voiceoverfirst");
        Invoke("startTyping", 4f); // This is because we have a 9 seconds late in the audio
    }

    void startTyping()
    {
        FindObjectOfType<AudioManager>().Play("Typing");
        twEffect.WriteText(input);
        Invoke("LoadPrototype", input.Length * 0.1f);
    }

    void LoadPrototype()
    {
        FindObjectOfType<AudioManager>().Stop("Typing");
        SceneManager.LoadScene("PrototypeScene");
    }
}
