using Assets.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionEffect : MonoBehaviour
{
    public GameObject potionObject;

    //Private fields
    float activeTimeCount;
    static bool isStronger;
    static bool isLoved;
    PlayerHealthManager healthManager;
    HurtEnemy hurtEnemy;
    HurtPlayer hurtPlayer;
    PlayerController playerController;

    void Start()
    {       
        healthManager = FindObjectOfType<PlayerHealthManager>();
        hurtEnemy = FindObjectOfType<HurtEnemy>();
        hurtPlayer = FindObjectOfType<HurtPlayer>();
    }

    public void DrunkPotion(Potion potion, GameObject player)
    {
        playerController = player.GetComponent<PlayerController>();

        GameObject getScript = playerController.gameObject;
        var transform = playerController.transform;
        healthManager = playerController.gameObject.GetComponent<PlayerHealthManager>();

        foreach (Transform value in playerController.transform)
        {
            if (value.gameObject.name == "swingBig") getScript = value.gameObject;
        }

        GameObject createGameObject;

        switch (potion.Name)
        {
            case "Healing":
                healthManager.HealPlayer(35);
                break;

            case "Stronger":
                hurtEnemy = getScript.GetComponentInChildren<HurtEnemy>();
                hurtEnemy.damage = 50;
                createGameObject = Instantiate(potionObject) as GameObject;
                break;

            case "Love":
                healthManager.PotionHealth(200);
                createGameObject = Instantiate(potionObject) as GameObject;
                break;

            case "TilTheEnd":
                healthManager.PotionHealth(2500);
                createGameObject = Instantiate(potionObject) as GameObject;
                break;

            case "Immunity":
                break;
        }
    }
}