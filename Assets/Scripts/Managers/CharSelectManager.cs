using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSelectManager : MonoBehaviour
{
    // Instancing the class
    public static CharSelectManager instance;

    // Variables charactermanager
    public PlayerController activePlayer;               // REF the active player controller
    public CharacterSelector activeCharSelect;          // REF the active character selector


    private void Awake()
    {
        instance = this;
    }
}
