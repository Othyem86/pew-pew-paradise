using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Variabels Menu
    public string levelToLoad;                          // REF Scene to load

    public GameObject deletePanel;                      // REF 'Delete Saved Game'-Panel
    public CharacterSelector[] charactersToDelete;      // REF Array of all unlockable Characters



    //
    //  METHODS
    //

    // Method start game
    public void StartGame()
    {
        SceneManager.LoadScene(levelToLoad);
    }



    // Method end program
    public void ExitGame()
    {
        Application.Quit();
    }



    // Method prompt player if he is sure
    public void DeleteSave()
    {
        deletePanel.SetActive(true);
    }



    // Method cancel delete command
    public void CancelDelete()
    {
        deletePanel.SetActive(false);
    }



    // Method delete character unlocks in PlayerPrefs
    public void ConfirmDelete()
    {
        foreach(CharacterSelector theCharacter in charactersToDelete)
        {
            PlayerPrefs.SetInt(theCharacter.playerToSpawn.name, 0);
        }

        deletePanel.SetActive(false);
    }
}