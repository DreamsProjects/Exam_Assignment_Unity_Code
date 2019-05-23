using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCDialogManager : MonoBehaviour
{
    public int currentLine;
    public string[] dialogLines;
    public bool isActive;
    public GameObject dialogBox;
    public Text text;

    //Private fields
    PlayerController player;
    QuestTrigger trigger;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        trigger = FindObjectOfType<QuestTrigger>();
    }

    void Update()
    {
        /*För att stänga ifall dialogen är öppen och spelaren klickar SPACE*/
        if (isActive)
        {
            if (currentLine >= dialogLines.Length)
            {
                isActive = false;
                dialogBox.SetActive(false);
                player.canMove = true;

                currentLine = 0; //reset
            }

            else
            {
                text.text = dialogLines[currentLine]; //Skriver ut rätt rad
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                currentLine++;
            }
        }
    }

    public void ShowDialogs(string values)
    {
        if (dialogBox != null)
        {
            dialogBox.SetActive(true);
            isActive = true;
            player.canMove = false;

            if (values == "")
            {
                text.text = dialogLines[currentLine];
            }

            else
            {
                text.text = values;
            }
        }

    }

    public void StatusText()
    {
        ShowDialogs("You need to finish your first quest before starting a new one");
    }
}