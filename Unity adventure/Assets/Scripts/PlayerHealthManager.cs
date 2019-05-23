using Assets.Repository;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    public int playerMaxHealth;
    public int playerCurrentHealth;
    public float flashLenght;
    public DeathMenu menuWhenDead;
    public SpriteRenderer sprite;
    public float activationTimePotion;
    public string potion;


    //Private fields
    public PlayerController player;
    ManagerGame game;
    float RespawnTime;
    float flashCounter;
    bool flashActive;

    void Start()
    {
        //Läs in Spelarens Stats/HP här
        var playerHealth = Generator.PlayerData();

        playerCurrentHealth = playerHealth.CurrentHp;
        playerMaxHealth = playerHealth.MaximumHp;
        flashLenght = 1;

        sprite = GetComponent<SpriteRenderer>();
        player = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (flashActive)
        {
            if (flashCounter > flashLenght * .66f)
            {
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.6f); //halvt osynlig
            }

            else if (flashCounter > flashLenght * 0.33f)
            {
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f); //synlig
            }


            else if (flashCounter > 0f)
            {
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.6f); //halv osynlig
            }

            else
            {
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f); //synlig

                flashActive = false;
            }

            flashCounter -= Time.deltaTime;
        }
    }

    public void HurtPlayer(int damage)
    {
        playerCurrentHealth -= damage;
        flashActive = true;
        flashCounter = flashLenght;

        PlayerDatabase.SetHealth(playerCurrentHealth);
    }

    public void SetMaxHealth()
    {
        playerCurrentHealth = playerMaxHealth;
        PlayerDatabase.SetHealth(playerCurrentHealth);
    }

    public void PotionHealth(int healing)
    {
        playerCurrentHealth += healing;
        PlayerDatabase.SetHealth(playerCurrentHealth);
    }

    public void HealPlayer(int healing)
    {
        if (playerCurrentHealth < playerMaxHealth)
        {
            playerCurrentHealth += healing;

            if (playerCurrentHealth >= playerMaxHealth)
            {
                SetMaxHealth();
            }
        }
    }
}