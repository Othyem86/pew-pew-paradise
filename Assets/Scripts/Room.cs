using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    // Variabeln Raumsteuerung
    public bool closedWhenEntered;                              // REF ob Raum sich beim Eingang schliessen soll
    [HideInInspector]
    public bool roomActive;                                     // REF ob der Raum aktiv ist
    public GameObject mapHider;                                 // REF Maske Raum für Karte
    public GameObject[] doors;                                  // REF Türliste


    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
       
    }


    // Funktion Raumtüren deaktivieren wenn keine Gegner
    public void OpenDoors()
    {
        closedWhenEntered = false;

        foreach (GameObject door in doors)
        {
            door.SetActive(false);
        }
    }


    //  Funktion Raum aktivieren, Kamera dorthin bewegen, wenn Spieler im Raum eintritt
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

            // Maske deaktivieren
            mapHider.SetActive(false);
        }
    }


    // Funktion Raum deaktivieren bei Spieleraustritt
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            roomActive = false;
        }
    }
}