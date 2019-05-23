using Assets.Classes;
using Assets.Repository;
using System;
using UnityEngine;

public class QuestTrigger : MonoBehaviour
{
    public string questItem;
    public int questNumber;
    public bool startQuest;
    public bool endQuest;
    public bool isStartByNPC;
    public bool isEnemyQuest;

    //Private fields
    BackpackController backpack;
    QuestHandler handler;

    void Start()
    {
        handler = FindObjectOfType<QuestHandler>();
        backpack = FindObjectOfType<BackpackController>();

        if (handler == null)
        {
            handler = GameObject.Find("Canvas").GetComponent<QuestHandler>();
        }

        if (backpack == null)
        {
            backpack = GameObject.Find("Canvas").GetComponent<BackpackController>();
        }

        Generator.Quests();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            CheckIfFinished();
        }
    }

    public void CheckIfFinished()
    {
        var noOtherActive = QuestRelated.ActivateQuest(questNumber);

        if (!QuestRelated.IsFinished(questNumber)) //kollar bara om questen inte är färdig 
        {
            if (noOtherActive) // är någon aktiv?
            {
                handler.quests[questNumber].gameObject.SetActive(true);//nuuuuuull
                handler.quests[questNumber].isActivated = true;

                if (!isStartByNPC)
                { handler.quests[questNumber].StartQuest(); } //Ska inte skriva ut hints då NPC berättar
            }

            else //Om aktiv quest
            {
                var returnedValue = CheckItem();

                if (returnedValue)
                {
                    if (!isStartByNPC && endQuest)
                    {
                        handler.quests[questNumber].EndQuest();
                        QuestRelated.ActiveQuest = null;
                    } //Ska inte skriva ut hints då NPC berättar

                    else if (isStartByNPC && endQuest)
                    {
                        handler.quests[questNumber].EndQuestNPC(); // nuuuuuuuuuuuuuuuull
                        QuestRelated.ActiveQuest = null;
                    }

                    //Ta bort föremål från ryggsäck
                    if (questItem != "" && questItem != "Staff" && questItem != "BlueWand")
                    {
                        backpack.RemoveQuestItem(questItem);
                    }
                }

                else if (endQuest && questItem == "" && !isEnemyQuest) //Om avslut och inget föremål krävs ---Ska också ha med monster att göra kanske?
                {
                    handler.quests[questNumber].EndQuest();
                    QuestRelated.ActiveQuest = null;
                }

                else if (endQuest && isEnemyQuest) //Fiende quest -KilledAllEnemies kollar om antalet stämmer i global lista
                {
                    handler.quests[questNumber].KilledAllEnemies();
                }
            }
        }
    }

    public bool CheckItem()
    {
        return backpack.HasRequestedItem(questItem);
    }
}