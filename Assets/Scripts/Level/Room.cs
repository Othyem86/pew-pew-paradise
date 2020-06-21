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
    public GameObject doors;                                    // REF array of all room doors



    //
    //  METHODS
    //

    // Deactivate all doors
    public void OpenDoors()
    {
        closedWhenEntered = false;
        doors.SetActive(false);
    }



    // Activate room, move camera to active room
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            CameraController.instance.ChangeCameraTarget(transform);

            // Activate room doors on player enter
            if (closedWhenEntered)
            {
                doors.SetActive(true);   
            }

            roomActive = true;

            // Deactivate room mask
            mapHider.SetActive(false);
        }
    }



    // Deactivate room on player exit
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            roomActive = false;
        }
    }
}