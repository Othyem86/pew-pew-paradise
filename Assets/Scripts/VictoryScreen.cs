using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour
{
    // Variablen Gewonnen Bildschirm
    public float waitForAnyKey = 2f;    // REF Wartezeit
    public GameObject anyKeyText;       // REF Text beliebiger Tastendruck
    public string mainMenuScene;        // REF Szene Hauptmenü


    // Start is called before the first frame update
    void Start()
    {
        // Zeitverlauf auf 100% setzen
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        // Wenn Countdown nicht null, Countdown starten oder fortsetzen
        if (waitForAnyKey > 0)
        {
            waitForAnyKey -= Time.deltaTime;
            if (waitForAnyKey <= 0)
            {
                anyKeyText.SetActive(true);
            }
        }
        else 
        {
            // Bei jedem Tastendruck zu Hauptmenüszene gehen
            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene(mainMenuScene);
            }
        }
    }
}
