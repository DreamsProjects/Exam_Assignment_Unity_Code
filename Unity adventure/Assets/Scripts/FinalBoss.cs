using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    public GameObject spittingPrefab;
    public Rigidbody2D enemyRigidBody;
    public Rigidbody2D playerRigidBody;

    //private fields
    float time;
    int setTime;
    int countFires;
    EnemyHealthManager health;
    ManagerGame game;

    void Start()
    {
        setTime = Random.Range(2, 5);
        time = (float)setTime;
        health = FindObjectOfType<EnemyHealthManager>();
        game = FindObjectOfType<ManagerGame>();
    }

    void Update()
    {
        time -= Time.deltaTime;

        if (time <= 0)
        {
            setTime = Random.Range(2, 5);
            time = (float)setTime;

            //Gör en ny skjutning av gameobjekt mot spelaren
            if (countFires >= 3)
            {
                countFires = 0;
                Instantiate(spittingPrefab, enemyRigidBody.transform.position, enemyRigidBody.transform.rotation);
                time = 0.5f;
            }

            else
            {
                Instantiate(spittingPrefab, enemyRigidBody.transform.position, enemyRigidBody.transform.rotation);
            }

            countFires++;
        }

        if(health.enemyCurrentHealth <= 0)
        {
            game.Finished();
        }
    }
}