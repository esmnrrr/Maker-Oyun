using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AkceVerici : MonoBehaviour
{
    public int akceMiktari = 1;
    private bool oyuncuYakinda = false;
    private bool verildi = false;

    public GameObject interactionText; // TMP yazısı atanacak

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !verildi)
        {
            oyuncuYakinda = true;
            if (interactionText != null)
                interactionText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            oyuncuYakinda = false;
            if (interactionText != null)
                interactionText.SetActive(false);
        }
    }

    private void Update()
    {
        if (oyuncuYakinda && !verildi && Input.GetKeyDown(KeyCode.E))
        {
            Inventory.instance.AddAkce(akceMiktari);
            verildi = true;

            if (interactionText != null)
                interactionText.SetActive(false);

            Debug.Log("E ile akçe alındı.");
        }
    }
}
