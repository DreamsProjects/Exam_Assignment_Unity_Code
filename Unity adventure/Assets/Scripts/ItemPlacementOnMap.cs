using Assets.Classes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlacementOnMap : MonoBehaviour
{
    public GameObject ItemsGameObject;

    public void OnDrop(Inventory inventory, GameObject playerObject)
    {
        if (inventory.tagName != "Item")
        {
            if (ItemsGameObject == null)
            {
                ItemsGameObject = (GameObject)Resources.Load($"Prefab/{inventory.nameOnItem}", typeof(GameObject));
            }

            var getPlayerController = playerObject.GetComponent<PlayerController>();
            var location = playerObject.transform.position;

            if (getPlayerController.currentMap == "") getPlayerController.currentMap = "start";

            KilledEnemiesPickedAndDropedItems.AddToDropped(inventory, getPlayerController.currentMap, location);

            ItemsGameObject.gameObject.transform.position = location;

            var activate = Instantiate(ItemsGameObject);
            activate.SetActive(true);
        }
    }

    public void CreateGameObject(GameObject component)
    {
        var activate = Instantiate(component); //Skapar en klon
        activate.SetActive(true);
    }
}