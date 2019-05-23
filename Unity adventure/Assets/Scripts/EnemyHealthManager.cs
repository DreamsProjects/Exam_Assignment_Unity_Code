using Assets.Classes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{
    /*Detta script håller koll på fiendens HP*/
    public int enemyMaxHealth;
    public int enemyCurrentHealth;
    public int enemyQuestNr;

    //Private fields
    QuestHandler handler;
    ManagerGame game;

    void Start()
    {
        enemyCurrentHealth = enemyMaxHealth;
        handler = FindObjectOfType<QuestHandler>();
        game = FindObjectOfType<ManagerGame>();
    }

    void Update()
    {
        if (enemyCurrentHealth <= 0)
        {
            KilledEnemiesPickedAndDropedItems.AddToKillList(gameObject.name);

            var name = gameObject.name;

            Destroy(gameObject);

            if (name == "tree_rock_magical")
            {
                game.finishedBox.SetActive(true);
            }
        }
    }

    public void HurtEnemy(int damage)
    {
        enemyCurrentHealth -= damage;
    }

    public void SetMaxHealth()
    {
        enemyCurrentHealth = enemyMaxHealth;
    }

    public int GetFullHp()
    {
        return enemyMaxHealth;
    }
}