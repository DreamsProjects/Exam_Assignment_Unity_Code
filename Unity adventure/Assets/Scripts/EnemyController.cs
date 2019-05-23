using Assets.Classes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed;
    public float timeBetweenMove;
    public float timeToMove;
    public float waitToReload;

    //Private fields
    Rigidbody2D myRigidBody;
    GameObject player;
    Vector3 moveDirection;
    List<InventoryAndKilledEnemies> killedEnemies = new List<InventoryAndKilledEnemies>();
    bool moving;

    //Private fields
    float timeBetweenMoveCounter;
    float timeToMoveCounter;
    bool reloading;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
        timeToMoveCounter = Random.Range(timeToMove * 0.75f, timeBetweenMove * 1.25f); //timeToMove

        killedEnemies = KilledEnemiesPickedAndDropedItems.killedEnemies;

        if (killedEnemies.Count > 0)
        {
            for (int i = 0; i < killedEnemies.Count; i++)
            {
                Destroy(GameObject.Find(killedEnemies[i].Name));
            }
        }
    }

    void Update()
    {
        if (moving)
        {
            timeToMoveCounter -= Time.deltaTime;
            myRigidBody.velocity = moveDirection;

            if (timeToMoveCounter < 0f)
            {
                moving = false;
                timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
            }
        }

        else
        {
            timeBetweenMoveCounter -= Time.deltaTime;
            myRigidBody.velocity = Vector2.zero;

            if (timeBetweenMoveCounter < 0f)
            {
                moving = true;
                timeToMoveCounter = Random.Range(timeToMove * 0.75f, timeBetweenMove * 1.25f); //timeToMove
                moveDirection = new Vector3(Random.Range(-1f, 1f) * moveSpeed, Random.Range(-1f, 1f) * moveSpeed, 0f);
            }
        }

        if (reloading)
        {
            waitToReload -= Time.deltaTime;

            if (waitToReload < 0f)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                player.SetActive(true);
            }
        }
    }
}