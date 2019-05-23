using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestHandler : MonoBehaviour
{
    public string itemCollected;
    public string enemyKilled;
    public Quests[] quests;
    public NPCDialogManager dialog;

    public void Start()
    {
        if(quests == null)
        {
            var quest = FindObjectOfType<Quests>();
            var allObjects = Resources.FindObjectsOfTypeAll(typeof(Quests)) as Quests[];

            quests = allObjects;
        }
    }

    public void ShowTextForQuest(string text)
    {
        dialog.dialogLines = new string[1];
        dialog.dialogLines[0] = text;
        dialog.currentLine = 0;
        dialog.ShowDialogs("");
    }
}