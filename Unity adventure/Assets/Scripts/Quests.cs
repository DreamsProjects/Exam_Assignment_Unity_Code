using Assets.Classes;
using Assets.Repository;
using System.Linq;
using UnityEngine;

public class Quests : MonoBehaviour
{
    public string startText;
    public string endText;
    public string targetItem;
    public string enemyName;
    public int QuestNr;
    public int howManyEnemies;
    public bool isItem;
    public bool isActivated;
    public bool isKillingQuest;
    public QuestHandler handler;

    //Private fields
    int killCount;

    void Update()
    {
        if (isItem) //Om föremåls-quest
        {
            if (targetItem == handler.itemCollected)
            {
                handler.itemCollected = null;
                EndQuest();
            }
        }

        if (isKillingQuest) //Om slå monster quest
        {
            if (handler.enemyKilled == enemyName)
            {
                handler.enemyKilled = null;

                killCount++;

                if (killCount >= howManyEnemies)
                {
                    EndQuest();
                }
            }
        }
    }

    public void KilledAllEnemies()
    {
        var howMany = KilledEnemiesPickedAndDropedItems.killedEnemies.Where(x => x.Name == enemyName).Count();  //killedEnemies

        if (howMany == howManyEnemies)
        {
            MakeToClass();
        }
    }

    public void StartQuest()
    {
        gameObject.SetActive(true);
        isActivated = true;
        handler.ShowTextForQuest(startText);
        QuestRelated.ActivateQuest(QuestNr);
        QuestDatabase.ActiveQuest(QuestNr);
    }

    public void TextQuestIfNotActivated()
    {
        handler.ShowTextForQuest("Finish your currect quest before starting a new one");
    }

    public void EndQuest()
    {
        handler.ShowTextForQuest(endText);
        MakeToClass();
    }

    public void EndQuestNPC()
    {
        MakeToClass();
    }

    public void GetQuestText()
    {
        handler.ShowTextForQuest(startText);
    }

    public void MakeToClass()
    {
        var quest = new QuestNumber
        {
            Number = QuestNr
        };
        QuestRelated.CompletedQuest.Add(quest);
        QuestRelated.ActiveQuest = null;
        //gameObject.SetActive(false);
        QuestDatabase.AddQuest(QuestNr);
        QuestDatabase.NoActiveQuest(QuestNr);
    }
}