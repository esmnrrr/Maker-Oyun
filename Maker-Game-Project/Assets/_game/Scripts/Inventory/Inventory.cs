using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public int akce = 0;

    public event Action OnInventoryChanged;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void AddAkce(int amount)
    {
        akce += amount;
        OnInventoryChanged?.Invoke();
        Debug.Log("Akçe eklendi: " + amount + " | Toplam: " + akce);
    }

    public void RemoveAkce(int amount)
    {
        akce = Mathf.Max(0, akce - amount);
        OnInventoryChanged?.Invoke();
    }

    public int GetAkce()
    {
        return akce;
    }
}
