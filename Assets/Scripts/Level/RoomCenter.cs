using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCenter : MonoBehaviour
{
    // Variables doors
    public bool openWhenEnemiesCleared;                         // REF if room should open doors on enemies cleared
    public List<GameObject> enemies = new List<GameObject>();   // REF list of all enemies 

    // Variabeln Raumkomponierung
    public Room theRoom;                                        // REF the room that envelops the room center


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
        TriggerOpenRoomDoors();
    }



    //
    //  METHODS
    //

    // Trigger open room doors when no more enemies in room
    private void TriggerOpenRoomDoors()
    {
        // Check if there are any enemies stil in the room
        if (enemies.Count > 0 && theRoom.roomActive && openWhenEnemiesCleared)
        {
            // If an enemy is destroyed (null values in list), remove said index from the list
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] == null)
                {
                    enemies.RemoveAt(i);
                    i--;
                }
            }


            // Deactivate doors when no more enemies
            if (enemies.Count == 0)
            {
                theRoom.OpenDoors();
            }
        }
    }
}
