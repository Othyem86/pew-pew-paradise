using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    // Variaben Charakterwahl
    private bool canSelect;                             // Ob man es auswählen kann
    public GameObject message;                          // REF Auswahlnachricht
    public PlayerController playerToSpawn;              // REF Charakter der geladen werden soll
    public bool shouldUnlock;                           // REF Ob Charakter befreibar ist

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
    // METHODEN
    //

    // Methode Charakter wechseln
    private void SwitchCharacter()
    {
        if (canSelect && Input.GetKeyDown(KeyCode.E))
        {
            // Spielerposition speichern und Spielerobjekt zerstören
            Vector3 playerPositon = PlayerController.instance.transform.position;
            Destroy(PlayerController.instance.gameObject);

            // Neuer Spieler instantieren und der Controlelrinstanz gleichsetzen
            PlayerController newPlayer = Instantiate(playerToSpawn, playerPositon, playerToSpawn.transform.rotation);
            PlayerController.instance = newPlayer;

            // Kamera auf neuen Spieler fixieren, Charakterselector deaktivieren
            gameObject.SetActive(false);
            CameraController.instance.target = newPlayer.transform;

            // Neuer Charakterselector als aktiv bezeichnen, alter Charakterselektor wieder aktivieren
            CharSelectManager.instance.activePlayer = newPlayer;
            CharSelectManager.instance.activeCharSelect.gameObject.SetActive(true);
            CharSelectManager.instance.activeCharSelect = this;
        }
    }



    // Methode Spieler neben gewünschten Charakter
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canSelect = true;
            message.SetActive(true);
        }
    }



    // Methode Spieler nicht neben gewünschten Charakter
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canSelect = false;
            message.SetActive(false);
        }
    }
}