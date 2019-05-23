using Assets.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Classes
{
    public class InventoryAndKilledEnemies
    {
        public string Name { get; set; }
    }

    public static class KilledEnemiesPickedAndDropedItems
    {
        public static List<InventoryAndKilledEnemies> killedEnemies = new List<InventoryAndKilledEnemies>();
        public static List<ItemOnGround> itemsMapName = new List<ItemOnGround>(); //UPP-plockade föremål
        public static List<Inventory> inventories = new List<Inventory>();
        private static ItemPlacementOnMap PlacementOnTheMap = new ItemPlacementOnMap();
        private static bool hasReadFromDatabase;

        public static void AddToKillList(string value)
        {
            if (value.Contains("skeleton"))
            {
                value = "Skeleton";
            }

            var addToList = new InventoryAndKilledEnemies { Name = value };
            killedEnemies.Add(addToList);

            if (!value.Contains("slime"))
            {
                //Spara ner till databasen
                EnemiesDatabase.AddEnemies(value);
            }
        }

        //Plockar upp föremål från marken
        public static void AddToItemList(string value, Item item, PlayerController player, Vector2 itemPosition)
        {
            var category = "";

            var map = new ItemOnGround
            {
                Name = item.Name,
                Amount = (int?)item.Amount ?? 1,
                Map = player.currentMap,
                Position = itemPosition
            };

            if (item.Food.Count > 0) category = "Food";
            if (item.Potion.Count > 0) category = "Potion";

            itemsMapName.Add(map);

            InventoryDatabase.AddToInventory(item.Name, item.IconPath, category, item.Tag);
            InventoryDatabase.RemoveItemsFromMap(item.Name, player.currentMap, itemPosition.ToString());
        }

        //Lägger ner föremål till kartan
        public static void AddToDropped(Inventory inventory, string onMap, Vector2 location)
        {
            inventory.replacedName = inventory.nameOnItem; //Försvinner annars...
            inventory.currentMap = onMap;
            inventory.placement = location;

            inventories.Add(inventory);

            InventoryDatabase.AddItemsToMap(inventory.replacedName, onMap, location.ToString());
            InventoryDatabase.RemoveFromInventory(inventory.replacedName);
        }

        public static void CreateGameObjectWhenLoadingMap(string currentMap)
        {
            if(inventories.Count <= 0)
            {
                Generator.ItemsOnMap();
            }

            foreach (var item in inventories.Where(x => x.currentMap == currentMap)) //Om inte i positions så är förmålet inte upp-plockat
            {
                var component = (GameObject)Resources.Load($"Prefab/{item.replacedName}", typeof(GameObject));
                component.gameObject.transform.position = item.placement;
                PlacementOnTheMap.CreateGameObject(component);
                component.gameObject.SetActive(true);
            }
        }

        public static void RemoveFromInventories(string currentMap, string name, Vector2 location)
        {
            if (inventories.Count > 0)
            {
                for (int i = 0; i < inventories.Count; i++)
                {
                    var hasInfo = inventories[i].placement == location && inventories[i].currentMap == currentMap && inventories[i].replacedName == name;

                    if (hasInfo)
                    {
                        inventories.RemoveAt(i);
                    }
                }
            }
        }
    }
}