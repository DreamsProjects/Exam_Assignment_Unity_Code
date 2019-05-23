using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public ManagerGame game;
    public GameObject lostConnection;

    //GameManager inaktiverar objektet vid klick
    public void Resume()
    {
        Time.timeScale = 1f;

        game.Resume();      
    }

    public void Pause()
    {
        Time.timeScale = 0f;
    }
    //Från början till mainmenu
    public void ToMainMenu()
    {
        //Spara ner till databasen
        Time.timeScale = 1f;
        //SceneManager.LoadScene("mainMenu");
        Application.Quit();
    }
}
