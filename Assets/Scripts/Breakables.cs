using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class Breakables : MonoBehaviour
{
    // Variabeln für Trümmer
    public GameObject[] brokenPieces;           // REF Array der Bruchteile
    public int maxPieces = 5;                   // REF Maximale Anzahl der Bruchteile


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
        // Zwischen 1 und 6 Trümmer generieren
        if (other.tag == "Player" && PlayerController.instance.dashCounter > 0)
        {
            Destroy(gameObject);
            int piecesToDrop = Random.Range(1, maxPieces);

            for (int i = 0; i < piecesToDrop; i++)
            {
                int randomPiece = Random.Range(0, brokenPieces.Length);
                Instantiate(brokenPieces[randomPiece], transform.position, transform.rotation);
            }
        }
    }
}
