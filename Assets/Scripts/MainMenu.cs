using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Variabeln Menu
    public string levelToLoad;      // REF Welche Szene geladen werden soll


    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    // Funktion Start Knopf
    public void StartGame()
    {
        SceneManager.LoadScene(levelToLoad);
    }


    // Funktion Programm beenden
    public void ExitGame()
    {
        Application.Quit();
    }
}
