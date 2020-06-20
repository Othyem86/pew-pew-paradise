using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    // Variable room control
    public bool closedWhenEntered;                              // REF if room should close doors on player enter
    [HideInInspector]
    public bool roomActive;                                     // REF if room is active
    public GameObject mapHider;                                 // REF mask for big map and minimap
    public GameObject[] doors;                                  // REF array of all room doors



    //
    //  METHODEN
    //

    // Method deactivate all doors
    public void OpenDoors()
    {
        closedWhenEntered = false;

        foreach (GameObject door in doors)
        {
            door.SetActive(false);
        }
    }


    // Method activate room, move camera to active room
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            CameraController.instance.ChangeCameraTarget(transform);

            // activate room doors on player enter
            if (closedWhenEntered)
            {
                foreach (GameObject door in doors)
                {
                    door.SetActive(true);
                }
            }

            roomActive = true;

            // deactivate room mask
            mapHider.SetActive(false);
        }
    }


    // Method deactivate room on player exit
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            roomActive = false;
        }
    }
}