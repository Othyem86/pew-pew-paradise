using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string name;                 // REF NPC name
    [TextArea(3, 10)]
    public string[] sentences;          // REF array of all dialogue sentences textfields
    public AudioSource[] sfx;           // REF array of all dialogue sound effects
}
