using Assets.Classes;
using Assets.Models;
using System.Collections.Generic;

namespace Assets.Repository
{
    //Spelaren, kameran, backpack;canvas 
    public static class Generator
    {
        public static Player ReadValues()
        {
            ItemsOnMap();
            KilledEnemies();
            QuestDatabase.DoneQuests(); //läser in klara quest

            return PlayerData();
        }

        //Läs in spelaren + HP
        public static Player PlayerData()
        {
            //Hämta spelarinformationen från PlayerDatabase
            var player = PlayerDatabase.PlayerStats();
            return player;
        }

        //Läggs till i Quest
        public static void Quests()
        {
            QuestDatabase.AnyActivated();
            QuestDatabase.DoneQuests();
        }

        //Läs in till Globala listan med OnMap i KilledEnemiesPickedAndDropedItems
        public static void ItemsOnMap()
        {
            var onGroundItems = InventoryDatabase.ItemsOnMap();

            foreach (var items in onGroundItems)
            {
                var invent = new Inventory
                {
                    currentMap = items.Map,
                    placement = items.Position,
                    nameOnItem = items.Name,
                    replacedName = items.Name
                };

                KilledEnemiesPickedAndDropedItems.inventories.Add(invent);
            }
        }

        //Läser in alla chests som öppnats från ChestDatabase --Sköts i BackpackController
        public static List<Chest> OpenedChests()
        {
            var getAllChests = ChestDatabase.OpenedChests();
            return getAllChests;
        }

        public static void ChestOnOpened(string map)
        {
            ChestDatabase.AddChest(map);
        }

        //Läser in alla dödade fiender till killedEnemies i KilledEnemiesPickedAndDropedItems
        public static void KilledEnemies()
        {
            KilledEnemiesPickedAndDropedItems.killedEnemies = EnemiesDatabase.ReadKilledEnemies();
        }
    }
}