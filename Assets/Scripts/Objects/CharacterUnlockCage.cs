using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUnlockCage : MonoBehaviour
{
    // Variables unlock cage
    private bool canUnlock;                                 // If cage can be unlocked
    public GameObject message;                              // REF cage unlock message

    // Variabeln Charakter auswählen
    public CharacterSelector[] characterSelectors;          // REF array of all possible unlockable characters
    private CharacterSelector playerToUnlock;               // Character to be randomly chosen
    public SpriteRenderer cagedSR;                          // REF SpriteRenderer of the character to be randomly chosen



    // Start is called before the first frame update
    void Start()
    {
        // Generate a ranom character to unlock
        playerToUnlock = characterSelectors[Random.Range(0, characterSelectors.Length)];
        cagedSR.sprite = playerToUnlock.playerToSpawn.bodySR.sprite;
    }



    // Update is called once per frame
    void Update()
    {
        UnlockCage();
    }



    //
    //  METHODS
    //

    // Unlock cage
    private void UnlockCage()
    {
        if (canUnlock && Input.GetKeyDown(KeyCode.E))
        {
            PlayerPrefs.SetInt(playerToUnlock.playerToSpawn.name, 1);

            Instantiate(playerToUnlock, transform.position, transform.rotation);

            gameObject.SetActive(false);
        }
    }



    // Player near cage
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canUnlock = true;
            message.SetActive(true);
        }
    }



    // Player not near cage
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canUnlock = false;
            message.SetActive(false);
        }
    }
}
