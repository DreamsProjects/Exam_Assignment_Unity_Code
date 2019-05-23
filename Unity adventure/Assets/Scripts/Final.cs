using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Final : FinalBoss
{
    public float wandSpeed;
    public Rigidbody2D globeBody;
    public PlayerController player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        Vector3 playersLastPosition = new Vector3(-player.lastMove.x, -player.lastMove.y, 0f);

        globeBody.velocity = playersLastPosition * wandSpeed;
    }

    private void Update()
    {
        globeBody.transform.position += transform.forward * wandSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" || collision.gameObject.name == "Hand")
        {
            Destroy(gameObject);
        }
    }
}