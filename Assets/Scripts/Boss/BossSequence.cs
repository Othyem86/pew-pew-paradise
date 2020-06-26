using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossSequence
{
    [Header("Sequence")]
    public BossAction[] actions;            // REF array of boss actions
    public int endSequenceHealth;           // REF boss hitpoints value that triggers next sequence
}
