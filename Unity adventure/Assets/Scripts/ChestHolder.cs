using Assets.Classes;
using Assets.Repository;
using UnityEngine;

public class ChestHolder : MonoBehaviour
{
    public bool hasOpened;
    public string chest;
    public string scene;
    public string contains;
    public string Category;
    public int HealingFood;
    public string itemPath;
    public string tagName;

    //Private fields
    bool opened;
    string[] chests;
    BackpackController backpack;
    NPCDialogManager dialogManager;

    void Start()
    {
        backpack = FindObjectOfType<BackpackController>();
        dialogManager = FindObjectOfType<NPCDialogManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Hand") //Om man väljer att "öppna" kistan
        {
            var item = new Item
            {
                Name = contains,
                IconPath = itemPath,
                Tag = tagName
            };

            var chest = new Chest
            {
                Scene = scene,
            };

            var hasOpenedBefore = backpack.HasOpened(scene);          
            var hasSpace = backpack.BackPackHasSpace(item, scene);
            var category = "";
            //Om kistan inte har öppnats och om det finns plats
            if (!hasOpenedBefore && hasSpace)
            {
                hasOpened = true;


                switch (Category)
                {
                    case "Potion":
                        item.Potion.Add(new Potion
                        {
                            Name = contains,
                            ActiveTime = HealingFood
                        });
                        backpack.Adding(item);
                        category = "Potion";
                        break;

                    case "Food":
                        item.Food.Add(new Food
                        {
                            Name = contains,
                            Healing = HealingFood
                        });
                        backpack.Adding(item);
                        category = "Food";
                        break;

                    default:
                        backpack.Adding(item);
                        break;
                }

                backpack.OpenedChest(chest);
                Generator.ChestOnOpened(scene);
                InventoryDatabase.AddToInventory(item.Name, item.IconPath, category, item.Tag);
            }
        }
    }
}