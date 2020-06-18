using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Variabeln Menu
    public string levelToLoad;                          // REF Scene to load

    public GameObject deletePanel;                      // REF Delete Saved Game Panel
    public CharacterSelector[] charactersToDelete;      // REF Array of all unlockable Characters


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


    public void DeleteSave()
    {
        deletePanel.SetActive(true);
    }



    public void CancelDelete()
    {
        deletePanel.SetActive(false);
    }



    public void ConfirmDelete()
    {
        foreach(CharacterSelector theCharacter in charactersToDelete)
        {
            PlayerPrefs.SetInt(theCharacter.playerToSpawn.name, 0);
        }

        deletePanel.SetActive(false);
    }
}
