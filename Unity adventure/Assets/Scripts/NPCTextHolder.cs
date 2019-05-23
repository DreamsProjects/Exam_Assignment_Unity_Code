using Assets.Classes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCTextHolder : MonoBehaviour
{
    public string[] dialogLines;
    public string textIfCompleted; //meddelande om quest klar
    public int questNumber;

    //Private fields
    NPCDialogManager npcText;
    PlayerController player;
    string[] text = new string[1];

    void Start()
    {
        npcText = FindObjectOfType<NPCDialogManager>();
        player = FindObjectOfType<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            var completed = QuestRelated.IsFinished(questNumber);

            if (completed) //Om allt OK
            {
                Completed();
            }

            else if (!completed)
            {
                NotCompleted();
            }
        }
    }

    public void Completed()
    {
        text[0] = textIfCompleted;

        npcText.dialogLines = text;
        npcText.currentLine = 0;

        npcText.ShowDialogs("");
    }

    //Om inte questen är påbörjad
    public void NotCompleted()
    {
        if (!npcText.isActive)
        {
            npcText.dialogLines = dialogLines;

            npcText.currentLine = 0;

            npcText.ShowDialogs("");
        }
    }
}