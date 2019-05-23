using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Slider hpBar;
    public Text hpText;
    public PlayerHealthManager playerHp;

    //Private fields
    static bool UIExists;
    BackpackController backpack;

    void Start()
    {
        backpack = FindObjectOfType<BackpackController>();

        if (!UIExists)
        {
            UIExists = true;
            DontDestroyOnLoad(transform.gameObject);
            backpack.GetInventory();
        }

        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        hpBar.maxValue = playerHp.playerMaxHealth;
        hpBar.value = playerHp.playerCurrentHealth;
        hpText.text = $"HP: {playerHp.playerCurrentHealth} / {playerHp.playerMaxHealth}";

        if (playerHp.player.currentMap == "mainMenu")
        {
            Destroy(gameObject);
        }
    }
}