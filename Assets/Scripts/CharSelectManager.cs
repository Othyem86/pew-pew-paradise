using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSelectManager : MonoBehaviour
{
    // Instanzierung der Klasse
    public static CharSelectManager instance;

    // Variabeln Charaktermanager
    public PlayerController activePlayer;               // REF der aktive Spielercharakter
    public CharacterSelector activeCharSelect;          // REF der aktive Charakterwähler


    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {

    }
}
