using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUnlockCage : MonoBehaviour
{
    // Variabeln Käfig entriegeln
    private bool canUnlock;                     // Ob Käfig entriegelt werden kann
    public GameObject message;                  // REF Nachricht Käfig

    // Variabeln Charakter auswählen


    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canUnlock = true;
            message.SetActive(true);
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canUnlock = false;
            message.SetActive(false);
        }
    }
}
