using Assets.Classes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemPicker : MonoBehaviour
{
    public string Name;
    public string IconPath;
    public string Category;
    public int HealingFood;
    public GameObject gameItem;
    public string TagName;

    //Private fields
    List<ItemOnGround> itemMapName = new List<ItemOnGround>();
    ItemPlacementOnMap itemPlacement;
    BackpackController backpack;
    PlayerController player;
    Rigidbody2D itemPosition;

    void Start()
    {
        backpack = FindObjectOfType<BackpackController>();
        player = backpack.player.GetComponent<PlayerController>();
        itemPosition = GetComponent<Rigidbody2D>();

        itemMapName = KilledEnemiesPickedAndDropedItems.itemsMapName;

        if (itemMapName.Count > 0)
        {
            foreach (var item in itemMapName.Where(x => x.Name != "Berries"))
            {
                var found = GameObject.Find(item.Name);

                if (found != null)
                {
                    found.gameObject.SetActive(false);
                }
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Hand")
        {
            var item = new Item
            {
                Name = Name,
                IconPath = IconPath,
                Object = gameItem,
                Tag = TagName
            };

            switch (Category)
            {
                case "Potion":
                    item.Potion.Add(new Potion
                    {
                        Name = Name,
                        ActiveTime = HealingFood
                    });
                    break;

                case "Food":
                    item.Food.Add(new Food
                    {
                        Name = Name,
                        Healing = HealingFood
                    });
                    break;
            }

            var returned = backpack.Adding(item);

            if (returned)
            {
                if (item.Name.ToLower() != "berries" && item.Name.ToLower() != "mushrooms")
                {
                    KilledEnemiesPickedAndDropedItems.AddToItemList(item.Name, item, player, itemPosition.transform.position);
                    KilledEnemiesPickedAndDropedItems.RemoveFromInventories(player.currentMap,item.Name, itemPosition.transform.position);
                    Destroy(gameItem.gameObject);
                }

                else
                {
                    KilledEnemiesPickedAndDropedItems.AddToItemList(item.Name, item, player, new Vector3(0, 0, 0));
                    KilledEnemiesPickedAndDropedItems.RemoveFromInventories(player.currentMap, item.Name, new Vector3(0, 0, 0));
                }
            }
        }
    }
}