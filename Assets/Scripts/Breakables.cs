using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class Breakables : MonoBehaviour
{
    // Variabeln für Trümmer
    public GameObject[] brokenPieces;           // REF Array der Bruchteile
    public int maxPieces = 5;                   // REF Maximale Anzahl der Bruchteile

    // Variabeln für Random Drops
    public bool shouldDropItem;                 // REF ob es ein Drop geben soll
    public GameObject[] itemsToDrop;            // REF Array von Drops
    public float itemDropPercent;               // REF Chancen ein Drop zu erstellen


    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    // Funktion Kollision Box mit Spieler
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Originalobjekt zerstören und zwischen 1 und 6 Trümmer generieren
        if ( other.tag == "PlayerBullet" || other.tag == "Player" && PlayerController.instance.dashCounter > 0)
        {
            // Objekt Zerstören
            Destroy(gameObject);
            int piecesToDrop = Random.Range(1, maxPieces);
            AudioManager.instance.PlaySFX(0);


            // Trümmer zeigen
            for (int i = 0; i < piecesToDrop; i++)
            {
                int randomPiece = Random.Range(0, brokenPieces.Length);
                Instantiate(brokenPieces[randomPiece], transform.position, transform.rotation);
            }


            // Drop erstellen
            if (shouldDropItem)
            {
                // Zufallszahl zwischen 0-100 generieren
                float dropChance = Random.Range(0f, 100f);

                // Falls der Zufallszahl kleiner ist als die Chancen zum Drop => Drop erstellen
                if (dropChance < itemDropPercent)
                {
                    int randomItem = Random.Range(0, itemsToDrop.Length);
                    Instantiate(itemsToDrop[randomItem], transform.position, transform.rotation);
                }
            }
        }
    }
}
