using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkPotionForPrefab : MonoBehaviour
{
    public PlayerController player;
    public GameObject potionSystem;

    //Private fields
    GameObject instantiatedObject;
    PlayerHealthManager health;
    HurtEnemy hurtEnemy;
    float time;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        health = player.GetComponent<PlayerHealthManager>();
        hurtEnemy = player.swing.GetComponent<HurtEnemy>();
        time = 10;
        instantiatedObject = Instantiate(potionSystem, new Vector3(player.transform.position.x, player.transform.position.y, -5), player.transform.rotation);  //Vad för objekt, position och vilket rutation
    }

    void Update()
    {
        instantiatedObject.transform.position = new Vector2(player.transform.position.x, player.transform.position.y);
        time -= Time.deltaTime;

        if (time <= 0)
        {
            health.SetMaxHealth();
            hurtEnemy.damage = 5;
            Destroy(gameObject);
        }
    }
}