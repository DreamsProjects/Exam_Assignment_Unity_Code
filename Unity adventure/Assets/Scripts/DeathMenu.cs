using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public ManagerGame game;

    public void RestartGame()
    {
        //Spara ner till databasen innan respawn
        game.RespawnToGame();
    }

    public void ToMainMenu()
    {
        //Spara ner till databasen
        Application.Quit();
    }
}