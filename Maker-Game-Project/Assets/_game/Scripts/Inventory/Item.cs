using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Item
{
    public string itemName;
    public Sprite icon;
    public string description;

    public Item(string name, Sprite icon, string description)
    {
        this.itemName = name;
        this.icon = icon;
        this.description = description;
    }
}
