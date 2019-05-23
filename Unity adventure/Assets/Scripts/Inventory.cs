using Assets.Classes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public int howMany;
    public int activityTime;
    public string nameOnItem;
    public string CategoryName;
    public string background;
    public string currentMap;
    public string replacedName;
    public string tagName;
    public Text howManyCount;
    public GameObject GameObject;
    public Vector2 placement;

    //Private fields
    Image imageSprite;

    public void SetSprite()
    {
        var convertToSprite = Resources.Load<Sprite>(background);
        howMany = 1;
        howManyCount.text = "1";
        imageSprite = gameObject.GetComponent<Image>();
        imageSprite.sprite = convertToSprite;        
    }

    public void IsUsedUp()
    {
        nameOnItem = "";
        howManyCount.text = "";
        background = "";
        imageSprite = gameObject.GetComponent<Image>();
        imageSprite.sprite = Resources.Load<Sprite>("backGroundSprite");
    }
}