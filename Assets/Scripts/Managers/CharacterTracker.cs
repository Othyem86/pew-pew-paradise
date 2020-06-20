using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTracker : MonoBehaviour
{
    // Instancing the class
    public static CharacterTracker instance;

    // Variables tracked paramenters 
    public int currentHealth;                   // REF current hitpoints
    public int maxHealth;                       // REF current maximum hitpoints
    public int currentCoins;                    // REF current currency amount


    // Before Start()
    public void Awake()
    {
        instance = this;
    }
}
