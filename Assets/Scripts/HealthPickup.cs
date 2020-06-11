using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    // Variabeln Heilpaket
    public int healAmount = 1;                  // REF Wert der Heilung
    public float waitToBeCollected = 0.5f;      // REF Zeit bis es aktiviert werden kann


    // Update is called once per frame
    void Update()
    {   
        // Abwarten je nach waitToBeCollected bis der Heilpaket aktiviert werden kann
        if(waitToBeCollected > 0)
        {
            waitToBeCollected -= Time.deltaTime;
        }
    }



    // Spieler um healAmount heilen wenn er mit dem Objekt kollidiert
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && waitToBeCollected <= 0)
        {
            PlayerHealthController.instance.HealPlayer(healAmount);
            Destroy(gameObject);
            AudioManager.instance.PlaySFX(7);
        }
    }
}