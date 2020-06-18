using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Instanzierung der Klasse
    public static LevelManager instance;

    // Variabeln Szenenübergang
    [Header("Scene Transition")]
    public float waitToLoad = 3f;       // REF Dauer bis nächste Szene
    public string nextLevel;            // REF nächste Szene
    public Transform startPoint;        // REF Startpunkt des Spielers

    // Variabeln Spielpause
    [Header("Pause / Unpause")]
    public bool ispaused;               // REF ob eine Pause besteht

    // Variabeln Geldsystem
    [Header("Money Tracker")]
    public int currentCoins;            // REF aktuelle Geldfonds


    // Wie Start(), nur davor
    public void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        // Spieler zur Startposition verlegen
        PlayerController.instance.transform.position = startPoint.position;
        PlayerController.instance.canMove = true;

        // Geldfonds beim Start lt. CharacterTracker setzen
        currentCoins = CharacterTracker.instance.currentCoins;

        // Beim start des Levels, Zeitverlauf auf 100% setzen
        Time.timeScale = 1;

        // UI Update
        UIController.instance.coinText.text = currentCoins.ToString();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
    }



    //
    //  METHODEN
    //

    // Metode Coroutine zur wechseln der Szene
    public IEnumerator LevelEnd()
    {
        // Spielerbewegung ausschalten und Zeit abwarten vor Szenenwechsel
        AudioManager.instance.PlayLevelWin();
        PlayerController.instance.canMove = false;
        UIController.instance.StartFadeToBlack();
        yield return new WaitForSeconds(waitToLoad);

        // Aktuelle Hitpoins, maximale Hitpoints und Geldfonds speichern
        CharacterTracker.instance.currentCoins = currentCoins;
        CharacterTracker.instance.currentHealth = PlayerHealthController.instance.currentHealth;
        CharacterTracker.instance.maxHealth = PlayerHealthController.instance.maxHealth;

        // Szene wechesln
        SceneManager.LoadScene(nextLevel);
    }



    // Methode Spiel Pause
    public void PauseUnpause()
    {
        if (!ispaused)
        {
            UIController.instance.pauseMenu.SetActive(true);
            ispaused = true;
            Time.timeScale = 0;
        }
        else
        {
            UIController.instance.pauseMenu.SetActive(false);
            ispaused = false;
            Time.timeScale = 1;
        }
    }



    // Methode Geld einnehmen
    public void GetCoins(int amount)
    {
        currentCoins += amount;

        // UI Update
        UIController.instance.coinText.text = currentCoins.ToString();
    }
    
    

    // Methode Geld ausgeben
    public void SpendCoins(int amount)
    {
        currentCoins -= amount;

        if (currentCoins < 0)
        {
            currentCoins = 0;
        }

        // UI Update
        UIController.instance.coinText.text = currentCoins.ToString();
    }

}