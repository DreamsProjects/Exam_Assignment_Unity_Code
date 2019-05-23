using Assets.Models;
using Assets.Repository;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject creditBox;
    public GameObject helpBox;
    public GameObject startBox;
    public GameObject loginRegisterBox;
    public GameObject passwordField;
    public Text errorMessage;
    public Text Username;
    public Text Password;
    public ManagerGame game;

    //Private fields
    string passwordValue;
    bool loggedIn;

    //[DllImport("DllRender")]
    //public static extern void DatabaseFunction(string database);

    void Start()
    {
        game = FindObjectOfType<ManagerGame>();

        if (game != null)
        {
            Destroy(GameObject.Find("Canvas"));
            Destroy(GameObject.Find("GameManager"));
            Destroy(GameObject.Find("Main Camera"));
            Destroy(GameObject.Find("Player"));
            AccountDatabase.PlayerID = 0;
        }

        //DatabaseFunction("URI=file:" + Application.dataPath + @"\StreamingAssets\SqlDatabaseConnection.db");
    }


    public void StartGame()
    {
        //inloggad spelare
        if (AccountDatabase.PlayerID > 0 && loggedIn)
        {
            var playerContent = Generator.ReadValues(); //Läser in föremål på kartan och dödade fiender

            NewScene scene = new NewScene();

            scene.StartTheGame(playerContent.CurrentMap);
        }

        else
        {
            loginRegisterBox.SetActive(true);
        }
    }

    public void Login() //Tar fram login rutan och hanterar värdena
    {
        GetPassword();
        //DatabaseFunction("URI=file:" + Application.dataPath + @"\StreamingAssets\SqlDatabaseConnection.db");

        AccountDatabase.Login(Username.text, passwordValue);

        if (AccountDatabase.PlayerID > 0)
        {
            loggedIn = true;
            StartGame();
        }

        else
        {
            errorMessage.text = "Wrong username or password";
        }
    }

    public void Register() //Lägger in värdena i databasen
    {
        if (Password.text.Length < 5 && Username.text.Length < 4)
        {
            errorMessage.text = "You need a longer password(5). You need a longer username(4)";
        }

        else if (Password.text.Length < 5)
        {
            errorMessage.text = "You need a longer password. Minimum is 5";
        }

        else if (Username.text.Length < 4)
        {
            errorMessage.text = "You need a longer username. Minimum is 4";
        }

        else
        {
            GetPassword();

            var message = AccountDatabase.Register(Username.text, passwordValue);

            if (message.ToLower() == "error")
            {
                errorMessage.text = "USERNAME IS ALREADY TAKEN";
            }

            else
            {
                PlayerDatabase.AddPlayer();
                InventoryDatabase.ItemConnection("startItems", "", "", "");
                errorMessage.text = "Successfully created a player";
            }
        }
    }

    public void GetPassword()
    {
        passwordValue = GameObject.Find("passwordField").GetComponent<InputField>().text;
    }

    public void StartLogin()
    {
        loginRegisterBox.SetActive(true);
    }

    public void EndLogin()
    {
        loginRegisterBox.SetActive(false);
    }

    public void Credit()
    {
        creditBox.SetActive(true);
    }

    public void InactivateCredit()
    {
        creditBox.SetActive(false);
    }

    public void Help()
    {
        helpBox.SetActive(true);
    }

    public void InactivateHelp()
    {
        helpBox.SetActive(false);
    }
}