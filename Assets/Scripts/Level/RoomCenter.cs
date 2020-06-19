using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCenter : MonoBehaviour
{
    // Variabeln Türenlogik
    public bool openWhenEnemiesCleared;                         // REF Türen öffnen wenn keine Gegner da sind
    public List<GameObject> enemies = new List<GameObject>();   // Liste Gegner  

    // Variabeln Raumkomponierung
    public Room theRoom;                                        // REF zugehöriger Raum


    // Start is called before the first frame update
    void Start()
    {
        if (openWhenEnemiesCleared)
        {
            theRoom.closedWhenEntered = true;
        }
    }


    // Update is called once per frame
    void Update()
    {
        // Überprüfen ob es noch Gegner im Raum gibt
        if (enemies.Count > 0 && theRoom.roomActive && openWhenEnemiesCleared)
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
                theRoom.OpenDoors();
            }
        }
    }
}
