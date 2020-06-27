using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    [Header("Dialogue Content")]
    public string name;                                 // REF NPC name
    [TextArea(3, 10)]
    public string[] sentences;                          // REF array of all dialogue sentences textfields

    [Header("Dialogue Voice Over")]
    public AudioSource[] sfx;                           // REF array of all dialogue sound effects

    [Header("Dialogue Events")]
    public bool eventsAfterDialogue;                    // REF if events should happen after dialogue ends;
    public GameObject[] gameObjectsToDeactivate;        // REF array of gameonbjects to deactivate after dialogue ends
    public GameObject[] gameObjectsToActivate;          // REF array of gameonbjects to activate after dialogue ends
}
