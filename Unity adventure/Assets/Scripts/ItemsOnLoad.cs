using Assets.Classes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsOnLoad : MonoBehaviour
{
    public string currentMap;

    void Start()
    {
        KilledEnemiesPickedAndDropedItems.CreateGameObjectWhenLoadingMap(currentMap);
    }
}