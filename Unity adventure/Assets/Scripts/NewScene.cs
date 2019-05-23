using Assets.Classes;
using Assets.Repository;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewScene : MonoBehaviour
{
    public int questNumber;
    public bool isQuestRelated;
    public string levelToLoad;
    public string startPoint;

    //Private fields
    PlayerController player;
    PlayerHealthManager health;
    HurtEnemy hurtEnemy;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        health = player.GetComponent<PlayerHealthManager>();
        hurtEnemy = player.GetComponent<HurtEnemy>();
    }

    public void StartTheGame(string loadLevel)
    {
        SceneManager.LoadScene(loadLevel);
    }

    //SPARA SPELARENS POSITION TILL DATABASEN -- OM SCENEN INTE ÄR MAINMENU!!!!
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            if (isQuestRelated)
            {
                //Om Questen är gjord --Om scen så är quest satt på ingången så ingen check för föremål behövs
                if (QuestRelated.IsFinished(questNumber))
                {
                    if (QuestRelated.ActiveQuest == questNumber)
                    {
                        QuestRelated.ActiveQuest = null;
                    }

                    if (health.playerCurrentHealth > 100)
                    {
                        health.playerCurrentHealth = 100;
                    }

                    if (levelToLoad != "mainMenu")
                    {
                    }

                    else
                    {
                        AccountDatabase.PlayerID = 0;
                    }

                    GameObject getScript = player.gameObject;

                    foreach (Transform value in player.transform)
                    {
                        if (value.gameObject.name == "swingBig") getScript = value.gameObject;
                    }

                    hurtEnemy = getScript.GetComponentInChildren<HurtEnemy>();
                    hurtEnemy.damage = 5;

                    SceneManager.LoadScene(levelToLoad);
                    player.startPoint = startPoint;
                    player.currentMap = levelToLoad;
                }
            }

            else
            {
                if (health.playerCurrentHealth > 100)
                {
                    health.playerCurrentHealth = 100;
                }

                if (levelToLoad != "mainMenu")
                {
                }

                else
                {
                    AccountDatabase.PlayerID = 0;
                }

                SceneManager.LoadScene(levelToLoad);
                player.startPoint = startPoint;
                player.currentMap = levelToLoad;
            }
        }
    }

    public void OnRespawn(PlayerController player)
    {
        if (player.currentMap == null || player.currentMap == "") player.currentMap = "start";

        if (player.startPoint == null || player.startPoint == "") player.startPoint = "spawn";

        if (player.currentMap == "wonderlandQuestThree")
        {
            player.currentMap = "wonderland";
            player.startPoint = "questThree";
        }

        SceneManager.LoadScene(player.currentMap);
    }
}