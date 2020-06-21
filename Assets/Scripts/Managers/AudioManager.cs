using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Instancing the class
    public static AudioManager instance;

    // Variables Music
    public AudioSource levelMusic;              // REF level music
    public AudioSource gameOverMusic;           // REF game over muisc
    public AudioSource victoryMusic;            // REF victory music

    // Variables Sound Effects
    public AudioSource[] sfx;                   // REF array of all sound effects


    // Before Start()
    private void Awake()
    {
        instance = this;
    }



    //
    //  METHODS
    //

    // Play game over music
    public void PlayGameOver()
    {
        levelMusic.Stop();
        gameOverMusic.Play();
    }


    // Play victory music
    public void PlayLevelWin()
    {
        levelMusic.Stop();
        victoryMusic.Play();

    }


    // Play sound effect
    public void PlaySFX(int sfxToPlay)
    {
        sfx[sfxToPlay].Stop();
        sfx[sfxToPlay].Play();
    }
}
