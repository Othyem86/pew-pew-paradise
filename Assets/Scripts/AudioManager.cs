using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Instanzierung der Klasse
    public static AudioManager instance;

    // Variabeln Music
    public AudioSource levelMusic;              // REF Levelmusik
    public AudioSource gameOverMusic;           // REF Spielendemusik
    public AudioSource victoryMusic;            // REF Erfolgsmusik

    // Variabeln Sound Effects
    public AudioSource[] sfx;


    // Wie Start(), nur davor
    private void Awake()
    {
        instance = this;
    }



    //
    //  METHODEN
    //

    // Methode Spielendemusik spielen
    public void PlayGameOver()
    {
        levelMusic.Stop();
        gameOverMusic.Play();
    }


    // Methode Erfolgsmusik spielen
    public void PlayLevelWin()
    {
        levelMusic.Stop();
        victoryMusic.Play();

    }


    // Methode Sound Effect spielen
    public void PlaySFX(int sfxToPlay)
    {
        sfx[sfxToPlay].Stop();
        sfx[sfxToPlay].Play();
    }
}
