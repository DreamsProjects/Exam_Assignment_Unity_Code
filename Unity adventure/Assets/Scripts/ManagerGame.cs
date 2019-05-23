using Assets.Classes;
using Assets.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ManagerGame : MonoBehaviour
{
    public PlayerController player;
    public DeathMenu deathMenu;
    public PauseMenu pause;
    public GameObject helpBox;
    public GameObject potionBox;
    public BackpackController backpack;
    public SpriteRenderer sprite;
    public GameObject finishedBox;

    //private fields
    static bool exist; //RÖR EJ
    PlayerHealthManager health;
    QuestHandler handler;
    bool hasHelp;
    public bool hasPaused;
    bool hasPotionText;
    bool isLastLevel;
    QuestNumber number;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        health = FindObjectOfType<PlayerHealthManager>();
        handler = FindObjectOfType<QuestHandler>();
        backpack = FindObjectOfType<BackpackController>();

        if (!exist)
        {
            exist = true;
            DontDestroyOnLoad(transform.gameObject);
        }

        else
        {
            Destroy(gameObject);
        }

        number = new QuestNumber
        {
            Number = 9
        };

        finishedBox.SetActive(false);
        Help();
        potionBox.SetActive(false);
        pause.Resume();

    }

    void Update()
    {
        var currentHealth = health.playerCurrentHealth;

        if (currentHealth <= 0)
        {
            DeadPlayer();
        }

        if (player.currentMap.ToLower() == "castle" && !isLastLevel)
        {
            isLastLevel = true;
            backpack.LastLevel();
        }

        if (QuestRelated.CompletedQuest.Contains(number))
        {
            finishedBox.SetActive(true);
            player.moveSpeed = 0; //Spelaren ska inte kunna röra sig
        }


        if (Input.GetKeyDown(KeyCode.P))
        {
            pause.gameObject.SetActive(true);

            if (!hasPaused)
            {
                pause.Pause();
                hasPaused = true;
            }

            else
            {
                pause.Resume();
                hasPaused = false;
            }

        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            //Någon quest som är aktiverad?
            var activatedQuest = handler.quests.Where(x => x.isActivated == true).LastOrDefault() ?? null;

            if (activatedQuest)
            {
                if (activatedQuest.isActivated)
                {
                    player.myRigidBody.velocity = Vector2.zero;
                    player.playerMoving = false;
                    handler.ShowTextForQuest(activatedQuest.startText);
                }
            }

            //annars skrivs det ut en text på att man måste börja söka efter en ny uppgift att genomföra.
            else
            {
                player.playerMoving = false;
                handler.ShowTextForQuest("You need to start a new quest to get any information");
            }
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            if (!hasHelp)
            {
                Help();
            }

            else //Klicka på H för att stänga
            {
                InactivateHelp();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //SAVE TO DB -- Eller kör autosave varje 5 minut eller liknande
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (!hasPotionText)
            {
                PotionText();
            }

            else //Klicka på T för att stänga
            {
                InactivatePotionText();
            }
        }


        if (Input.GetKeyDown(KeyCode.Space)) //TODO: FIX Paus
        {
            var time = 20f;

            if (Time.timeScale == 1.0f)
                Time.timeScale = 0.0f;
            else
                Time.timeScale = 1.0f;

            while (time > 0f)
            {
                time -= Time.deltaTime;
            }

            Time.timeScale = 1.0f;
        }

        for (int i = 1; i < 7; i++)
        {
            if (Input.GetKeyDown("" + i))
            {
                backpack.WhatKey(i - 1);
            }
        }

        player.playerMoving = true;
    }

    public void Resume()
    {
        pause.gameObject.SetActive(false);
        hasPaused = false;
    }

    public void Finished()
    {
        player.canMove = false;
        finishedBox.SetActive(true);
    }

    public void RespawnToGame()
    {
        Time.timeScale = 1f;
        deathMenu.gameObject.SetActive(false);
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f); //synlig
        var scene = new NewScene();
        PlayerDatabase.SetHealth(100);
        health.SetMaxHealth();
        scene.OnRespawn(player);
    }

    public void DeadPlayer()
    {
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.6f); //halvt osynlig
        Time.timeScale = 0f;
        deathMenu.gameObject.SetActive(true);
    }

    public void PotionText()
    {
        Time.timeScale = 0f;
        potionBox.SetActive(true);
        hasPotionText = true;
    }

    public void InactivatePotionText()
    {
        Time.timeScale = 1f;
        potionBox.SetActive(false);
        hasPotionText = false;
    }

    public void Help()
    {
        Time.timeScale = 0f;
        helpBox.SetActive(true);
        hasHelp = true;
    }

    public void InactivateHelp()
    {
        Time.timeScale = 1f;
        helpBox.SetActive(false);
        hasHelp = false;
    }
}