using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class HotbarUI : MonoBehaviour
{
    public GameObject[] slotBorders = new GameObject[9];

    public Image[] slotImages = new Image[9]; // Slot0–Slot8'deki Image'lar
    public TextMeshProUGUI descriptionText;

    public Item[] items = new Item[9]; // Her slotun içindeki item

    private int selectedSlot = -1;

    void Start()
    {
        for (int i = 0; i < slotBorders.Length; i++)
        {
            if (slotBorders[i] != null)
                slotBorders[i].SetActive(false);
        }

        UpdateUI();
    }

    void Update()
    {
        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKeyDown((KeyCode)((int)KeyCode.Alpha1 + i)))
            {
                SelectSlot(i);
            }
        }
    }


    public void SelectSlot(int index)
    {
        if (index < 0 || index >= items.Length)
            return;

        selectedSlot = index;

        if (items[index] != null)
            descriptionText.text = $"[{index + 1}] {items[index].itemName}: {items[index].description}";
        else
            descriptionText.text = $"[{index + 1}] Boş slot";

        UpdateUI();
    }



    public void SetItem(int slotIndex, Item item)
    {
        items[slotIndex] = item;
        UpdateUI();
    }

    public void UpdateUI()
    {
        for (int i = 0; i < slotImages.Length; i++)
        {
            if (items[i] != null && items[i].icon != null)
            {
                slotImages[i].sprite = items[i].icon;
                slotImages[i].color = Color.white;

            }
            else
            {
                slotImages[i].sprite = null;
                slotImages[i].color = new Color(1, 1, 1, 0.2f); // Saydam
            }
            if (slotBorders[i] != null)
                slotBorders[i].SetActive(i == selectedSlot);
        }
    }

}
