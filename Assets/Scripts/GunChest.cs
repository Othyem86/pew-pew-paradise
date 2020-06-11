using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunChest : MonoBehaviour
{
    // Variabeln Waffentruhe
    [Header("General")]
    public SpriteRenderer theSR;                    // REF SpriteRenderer
    public Sprite chestOpen;                        // REF Sprite für offene Truhe
    public GameObject notification;                 // REF Textnachricht zum öffnen

    // Variabeln Waffen Drop
    [Header("Drops")]
    public Transform spawnPoint;                    // REF Erstellpunkt des Drops
    public GunPickup[] potentialGunDrops;           // REF Liste aller möglichen Drops
    public float scaleSpeed = 3f;                   // REF Geschwindigkeit Animation
    private bool canOpen;                           // Ob Spieler die Truhe öffnen kann
    private bool isOpen;                            // Ob Spieler die Truhe geöffnet hat


    // Update is called once per frame
    void Update()
    {
        OpenChest();
    }



    //
    // METHODEN
    //

    // Methode Truhe öffnen
    private void OpenChest()
    {
        if(canOpen && !isOpen)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                int gunDropSelect = Random.Range(0, potentialGunDrops.Length);

                Instantiate(potentialGunDrops[gunDropSelect], spawnPoint.position, spawnPoint.rotation);

                theSR.sprite = chestOpen;
                notification.SetActive(false);
                transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                isOpen = true;
            }
        }

        // Kurze Animation
        if (isOpen)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one, Time.deltaTime * scaleSpeed);
        }
    }



    // Method Spieler neben Waffentruhe
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !isOpen)
        {
            notification.SetActive(true);
            canOpen = true;
        }
    }



    // Methode Spieler nicht neben Waffentruhe
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            notification.SetActive(false);
            canOpen = false;
        }
    }
}
