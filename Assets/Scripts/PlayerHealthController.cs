using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    // Instanzierung der Klasse
    public static PlayerHealthController instance;

    public int currentHealth;       // REF aktuelle Hitpoints
    public int maxHealth;           // REF maximale Hitpoints


    // Wie Start(), nur davor
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // Funktion Schadenanrichtung an Spieler und dessen Tod
    public void DamagePlayer()
    {
        currentHealth--;

        if (currentHealth <= 0)
        {
            PlayerController.instance.gameObject.SetActive(false);
        }
    }
}
