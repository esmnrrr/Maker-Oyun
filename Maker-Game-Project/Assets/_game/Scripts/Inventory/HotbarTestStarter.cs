using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class HotbarTestStarter : MonoBehaviour
{
    public HotbarUI hotbarUI;
    public Sprite akceIcon;

    void Start()
    {
        Item akce = new Item("Akçe", akceIcon, "Deli Dumrul'un topladığı altın paradır.");
        hotbarUI.SetItem(0, akce); // Slot 1
    }
}
