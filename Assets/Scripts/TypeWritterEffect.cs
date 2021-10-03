using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TypeWritterEffect : MonoBehaviour
{
    public TextMeshProUGUI text;
    string inputText;
    string outputText;
    int typePosition;
    void Start()
    {
    }


    public void WriteText(string itext)
    {
        text.text = "";
        inputText = itext;
        outputText = "";
        typePosition = 0;
        Invoke("StartWritting", 0.1f);
    }

    void StartWritting()
    {
        if (typePosition < inputText.Length)
        {
            outputText += inputText[typePosition];
            typePosition++;
            text.text = outputText;
            Invoke("StartWritting", 0.1f);
        }
    }


}
