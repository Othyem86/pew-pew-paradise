using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUnlockCage : MonoBehaviour
{
    // Variabeln Käfig entriegeln
    private bool canUnlock;                                 // Ob Käfig entriegelt werden kann
    public GameObject message;                              // REF Nachricht Käfig

    // Variabeln Charakter auswählen
    public CharacterSelector[] characterSelectors;          // REF Liste der möglichen Charaktere
    private CharacterSelector playerToUnlock;               // Ausgewählter Charakter
    public SpriteRenderer cagedSR;                          // REF SpriteRenderer des ausgewählten Charakters



    // Start is called before the first frame update
    void Start()
    {
        playerToUnlock = characterSelectors[Random.Range(0, characterSelectors.Length)];
        cagedSR.sprite = playerToUnlock.playerToSpawn.bodySR.sprite;
    }



    // Update is called once per frame
    void Update()
    {
        if (canUnlock && Input.GetKeyDown(KeyCode.E))
        {
            PlayerPrefs.SetInt(playerToUnlock.playerToSpawn.name, 1);

            Instantiate(playerToUnlock, transform.position, transform.rotation);

            gameObject.SetActive(false);
        }
    }



    // Methode Spieler neben Käfig
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canUnlock = true;
            message.SetActive(true);
        }
    }



    // Methode Spieler nicht neben Käfig
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canUnlock = false;
            message.SetActive(false);
        }
    }
}
