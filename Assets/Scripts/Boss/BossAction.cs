using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Boss actions class
[System.Serializable]
public class BossAction
{
    [Header("Action")]
    public float actionLength;              // REF boss action duration

    [Header("Movement")]
    public bool shouldMove;                 // REF if boss should move
    public bool shouldChasePlayer;          // REF if boss should chase player
    public bool shouldMoveToWaypoint;       // REF if boss should move to waypoint
    public float moveSpeed;                 // REB boss movement speed
    public Transform waypoint;              // REF waypoint boss should move to

    [Header("Shoot Volley")]
    public bool shouldShoot;                // REF if boss should shoot
    public GameObject itemToShoot;          // REF bullet boss should shoot
    public float timeBetweenShots;          // REF duration until next volley
    public Transform[] shootingPoints;      // REF array of boss bullet origins

}