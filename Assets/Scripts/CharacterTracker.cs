using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTracker : MonoBehaviour
{
    // Instanzierung der Klasse
    public static CharacterTracker instance;

    // Variabeln Spielerparameter die zu verfolgen sind
    public int currentHealth;                   // REF aktuelle Hitpoints
    public int maxHealth;                       // REF aktuelle maximale Hitpoints
    public int currentCoins;                    // REF aktuelle Geldsaldo


    // Wie Start(), nur davor
    public void Awake()
    {
        instance = this;
    }
}
