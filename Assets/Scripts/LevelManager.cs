using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Instanzierung der Klasse
    public static LevelManager instance;

    // Variabeln Szenenübergang
    public float waitToLoad = 4f;       // REF Dauer bis nächste Szene
    public string nextLevel;            // REF nächste Szene

    // Variabeln Spielpause
    public bool ispaused;               // REF ob eine Pause besteht


    // Wie Start(), nur davor
    public void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        // Beim start des Levels, Zeitverlauf auf 100% setzen
        Time.timeScale = 1;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
    }


    // Co-routine Funktion zur wechseln der Szene
    public IEnumerator LevelEnd()
    {
        // Spielerbewegung ausschalten und Zeit abwarten vor Szenenwechsel
        AudioManager.instance.PlayLevelWin();
        PlayerController.instance.canMove = false;
        UIController.instance.StartFadeToBlack();
        yield return new WaitForSeconds(waitToLoad);
        SceneManager.LoadScene(nextLevel);
    }


    // Funktion Spiel Pause
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
}