using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    // Variabeln Türenlogik
    public bool closedWhenEntered;                              // REF sollte sich öffnen
    public bool openWhenEnemiesCleared;                         // REF Türen öffnen wenn keine Gegner da sind
    private bool roomActive;                                    // Ob der Raum jetzt aktiv ist
    public GameObject[] doors;                                  // REF Türliste
    public List<GameObject> enemies = new List<GameObject>();   // Liste Gegner  


    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        // Überprüfen ob es noch Gegner im Raum gibt
        if (roomActive && enemies.Count > 0 && openWhenEnemiesCleared)
        {
            // Wenn Gegner zerstört wird, von liste entfernen
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] == null)
                {
                    enemies.RemoveAt(i);
                    i--;
                }
            }


            // Raumtüren deaktivieren wenn keine Gegner
            if (enemies.Count == 0)
            {
                if (closedWhenEntered)
                {
                    foreach (GameObject door in doors)
                    {
                        door.SetActive(false);
                        closedWhenEntered = false;
                    }
                }
            }
        }
    }


    // Wenn Spieler im Raum eintritt, Kamera dorthin bewegen, Raum aktivieren
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            CameraController.instance.ChangeCameraTarget(transform);


            // Raumtüren aktivieren bei Kameraeintritt
            if (closedWhenEntered)
            {
                foreach (GameObject door in doors)
                {
                    door.SetActive(true);
                }
            }

            roomActive = true;
        }
    }


    // Raum deaktivieren bei Spieleraustritt
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            roomActive = false;
        }
    }
}
