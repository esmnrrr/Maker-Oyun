/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public TextMeshProUGUI akceText;

    private void Start()
    {
        Inventory.instance.OnInventoryChanged += UpdateAkceUI;
        UpdateAkceUI(); // başlangıçta da göster
    }

    public void UpdateAkceUI()
    {
        akceText.text = "Akçe: " + Inventory.instance.GetAkce();
    }
}
*/