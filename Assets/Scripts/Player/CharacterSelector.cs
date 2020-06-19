using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    // Variables choose characters
    private bool canSelect;                             // if character is selectable
    public GameObject message;                          // REF choose character message
    public PlayerController playerToSpawn;              // REF character object that should pe spawned
    public bool shouldUnlock;                           // REF if the character is unlockable


    // Start is called before the first frame update
    void Start()
    {
        if (shouldUnlock)
        {
            if (PlayerPrefs.HasKey(playerToSpawn.name))
            {
                if (PlayerPrefs.GetInt(playerToSpawn.name) == 1)
                {
                    gameObject.SetActive(true);
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        SwitchCharacter();
    }



    //
    // METHODS
    //

    // Method change character
    private void SwitchCharacter()
    {
        if (canSelect && Input.GetKeyDown(KeyCode.E))
        {
            // Save player positon and destroy the active player object
            Vector3 playerPositon = PlayerController.instance.transform.position;
            Destroy(PlayerController.instance.gameObject);

            // Instantiate new player and assign the PlayerController instance to it
            PlayerController newPlayer = Instantiate(playerToSpawn, playerPositon, playerToSpawn.transform.rotation);
            PlayerController.instance = newPlayer;

            // Fix camera on the newly created player and deactivate selected selectable character
            gameObject.SetActive(false);
            CameraController.instance.target = newPlayer.transform;

            // set new character as active, reactivate previous selectable character
            CharSelectManager.instance.activePlayer = newPlayer;                    // TO DO: scrap functionality
            CharSelectManager.instance.activeCharSelect.gameObject.SetActive(true);
            CharSelectManager.instance.activeCharSelect = this;
        }
    }



    // Method if player near selectable character
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canSelect = true;
            message.SetActive(true);
        }
    }



    // Method player not near selectable character
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canSelect = false;
            message.SetActive(false);
        }
    }
}