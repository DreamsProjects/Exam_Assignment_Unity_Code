using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class DropItemOnClick : MonoBehaviour
{
    public GameObject whatIndex;

    public void DropItem()
    {
        var backpack = GetComponentInParent<BackpackController>();

        var numbers = Regex.Split(whatIndex.gameObject.name, @"\D+");

        var getCorrectNumber = int.Parse(numbers[1]);

        backpack.DropExistingItem(getCorrectNumber);
    }
}
