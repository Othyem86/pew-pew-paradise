using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Dialogue
{
    [Header("Dialogue Content")]
    public Sprite npcPortrait;                          // REF NPC portrait
    public string name;                                 // REF NPC name
    public int dialogueSpeed;                           // REF Dialogue speed
    public float reduceMusicVolume;                     // REF music volume reduction while dialogue is active
    [TextArea(3, 10)]
    public string[] sentences;                          // REF array of all dialogue sentences textfields

    [Header("Dialogue Voice Over")]
    public AudioSource voiceSource;                     // REF Audiosource for the voice clips
    public AudioClip[] voiceOvers;                      // REF array of all dialogue voice clips

    [Header("Dialogue Events")]
    public bool eventsAfterDialogue;                    // REF if events should happen after dialogue ends;
    public GameObject[] gameObjectsToDeactivate;        // REF array of gameonbjects to deactivate after dialogue ends
    public GameObject[] gameObjectsToActivate;          // REF array of gameonbjects to activate after dialogue ends
}
