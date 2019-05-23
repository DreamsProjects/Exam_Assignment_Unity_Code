using Assets.Classes;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets.Repository;

public class BackpackController : MonoBehaviour
{
    public Inventory[] slots;
    public GameObject player;
    public GameObject healingAnim;
    public GameObject drinkPotionPrefab;

    //Private fields
    List<Chest> chests;
    bool inBackpack;
    bool hasSlots;
    PotionEffect effect;
    HealPlayer healPlayer;
    static bool AlreadyRead;

    void Start()
    {
        //läs in öppnade kistor här
        chests = new List<Chest>();
        var opened = Generator.OpenedChests();
        chests = opened;

        //läs in ryggsäcken från databasen
        if (slots == null) GetInventory();
    }

    public void GetInventory()
    {
        //läs in ryggsäcken från databasen
        var inventoryList = InventoryDatabase.PlayersInventory();

        if (inventoryList.Count > 0)
        {
            foreach (var item in inventoryList)
            {
                var itemInInventory = new Item
                {
                    Name = item.nameOnItem ?? "",
                    IconPath = item.background ?? "",
                    Tag = item.tagName ?? ""
                    //Object = (GameObject)Resources.Load($"{item.nameOnItem}"),
                };

                if (item.tagName.ToLower() != "item") itemInInventory.Object = (GameObject)Resources.Load($"{item.nameOnItem}");

                if (item.CategoryName == "Food")
                {
                    var health = 0;

                    if (item.nameOnItem.ToLower() == "berries") health = 5;
                    else health = 9;

                    var Food = new Food
                    {
                        Name = item.nameOnItem,
                        Healing = health
                    };

                    itemInInventory.Food.Add(Food);
                }

                if (item.CategoryName == "Potion")
                {
                    var potion = new Potion
                    {
                        ActiveTime = 10,
                        Name = item.nameOnItem
                    };

                    itemInInventory.Potion.Add(potion);
                }

                Adding(itemInInventory);
            }
        }
    }

    public bool HasRequestedItem(string name)
    {
        return slots.Where(x => x.nameOnItem.ToLower() == name.ToLower()).FirstOrDefault() != null;
    }

    public void LastLevel()
    {
        var hasWand = slots.Where(x => x.nameOnItem.ToLower() == "staff").FirstOrDefault();

        if (hasWand != null)
        {
            hasWand = null;
        }
    }

    public void Eating(Food food)
    {
        if (food.Healing <= 0) food.Healing = 9;

        healPlayer = new HealPlayer();
        healPlayer.damageNumber = healingAnim;
        healPlayer.HealPlayerWithFood(food.Healing, player);
    }

    public void Drinking(Potion potion)
    {
        effect = new PotionEffect();
        effect.potionObject = drinkPotionPrefab;//Animering
        effect.DrunkPotion(potion, player);
    }

    public void Drop(Inventory items)
    {
        var hasNeither = false;

        switch (items.tagName)
        {
            case "Item":
                hasNeither = true;
                break;
        }

        if (!hasNeither)
        {
            items.howMany--;
            items.howManyCount.text = items.howMany.ToString();

            if (items.howMany == 0)
            {
                items.IsUsedUp();
            }
        }
    }

    public void RemoveQuestItem(string name)
    {
        var inventoryItem = slots.Where(x => x.nameOnItem == name).FirstOrDefault();

        Removing(inventoryItem, true);
    }

    //Bara föremål som används. Gället alltså inte droppade förmål
    public void Removing(Inventory items, bool NpcGiven)
    {
        //Ska kolla om föremålet kan användas: Om NPCGiven tas föremålet bort. Om det är mat eller potion så kan det användas
        var isEdible = items.CategoryName == "Food" || items.CategoryName == "Potion";

        if (items != null && NpcGiven || isEdible)
        {
            var removing = slots.Where(x => x.nameOnItem == items.nameOnItem).FirstOrDefault();

            if (removing.nameOnItem.ToLower() == "staff" )
            {
                //SKA EJ KUNNA TAS BORT
            }

            else if (removing.CategoryName == "Potion")
            {
                var potion = new Potion
                {
                    Name = removing.nameOnItem,
                    ActiveTime = removing.activityTime,
                    Object = removing.gameObject
                };

                Drinking(potion);
                InventoryDatabase.RemoveFromInventory(removing.nameOnItem);
                items.howMany--;
                items.howManyCount.text = items.howMany.ToString();
            }

            else if (removing.CategoryName == "Food")
            {
                var food = new Food
                {
                    Name = removing.nameOnItem,
                    Healing = removing.activityTime,
                    Object = removing.gameObject
                };

                Eating(food);
                InventoryDatabase.RemoveFromInventory(removing.nameOnItem);
                items.howMany--;
                items.howManyCount.text = items.howMany.ToString();

                if (items.howMany == 0) //Annars tas inte det bort från ryggsäcken
                {
                    removing = null;
                    items.IsUsedUp();
                }
            }

            if (items.howMany == 0) //Från removing
            {
                removing = null;
                items.IsUsedUp();
            }

            else if (NpcGiven && removing.howMany > 0) //Ibland två frön i ryggsäcken
            {
                InventoryDatabase.RemoveFromInventory(removing.nameOnItem);
                removing = null;
                items.IsUsedUp();
            }
        }
    }

    public bool Adding(Item items)
    {
        //Kolla först om itemet finns i ryggsäcken
        //Annars koll om det finns en tom plats

        if (items.Name == "BlueWand")
        {
            var hasStaff = slots.Where(x => x.nameOnItem == "Staff").FirstOrDefault();
            hasStaff.IsUsedUp();
            hasStaff = null;

            for (int i = 0; i <= slots.Length; i++)
            {
                if (slots[i].howMany == 0 || slots[i].nameOnItem == "")
                {
                    HasSlotsAvalible(slots[i], items);
                    return true;
                }
            }
        }

        else
        {
            var hasItem = slots.Where(x => x.nameOnItem == items.Name).FirstOrDefault();

            if (hasItem == null) //Om item inte finns
            {
                for (int i = 0; i <= slots.Length; i++)
                {
                    if (slots[i].howMany == 0 || slots[i].nameOnItem == "")
                    {
                        HasSlotsAvalible(slots[i], items);
                        return true;
                    }
                }
            }

            else
            {
                IsAlreadyOnSlot(hasItem);
                return true;
            }
        }

        return false;
    }

    //Finns item redan
    public void IsAlreadyOnSlot(Inventory index)
    {
        index.howMany++;
        index.howManyCount.text = index.howMany.ToString();
        inBackpack = true;
    }

    //Om item inte finns, finns det plats?
    public void HasSlotsAvalible(Inventory index, Item items)
    {
        if (items.Food.Count > 0) { index.CategoryName = "Food"; index.activityTime = items.Food.FirstOrDefault().Healing; }
        else if (items.Potion.Count > 0) { index.CategoryName = "Potion"; index.activityTime = items.Potion.FirstOrDefault().ActiveTime; }

        index.nameOnItem = items.Name;
        index.background = items.IconPath;
        index.GameObject = items.Object;
        index.tagName = items.Tag;
        index.SetSprite();
        hasSlots = true;
    }

    public void WhatKey(int key)
    {
        var getInventoryIndex = slots[key];

        if (getInventoryIndex.howMany != 0)
        {
            Removing(getInventoryIndex, false);
        }
    }

    public bool HasOpened(string where)
    {
        if (chests != null)
        {
            var hasOpened = chests.Where(x => x.Scene == where).FirstOrDefault();

            if (hasOpened == null) return false;

            else return true;
        }

        return false;
    }

    public void OpenedChest(Chest chest)
    {
        chests.Add(chest);
    }

    public bool BackPackHasSpace(Item items, string locationOnChest)
    {
        var hasItem = slots.Where(x => x.nameOnItem == items.Name).FirstOrDefault();

        if (chests.Where(x => x.Scene == locationOnChest) != null) return true; //Om kistan redan öppnats så ska den inte gå vidare

        if (hasItem != null)
        {
            IsAlreadyOnSlot(hasItem); //Kollar om föremål finns i ryggsäcken så slipper man kolla plats
            return true;
        }

        else
        {
            for (int i = 0; i <= slots.Length; i++)
            {
                if (slots[i].howMany == 0)
                {
                    return true;
                }
            }

            return false;
        }
    }

    public void DropExistingItem(int index)
    {
        var hasItem = slots[index];

        if (hasItem != null)
        {
            var itemPlacement = new ItemPlacementOnMap();
            itemPlacement.OnDrop(hasItem, player);

            Drop(hasItem);
        }
    }
}