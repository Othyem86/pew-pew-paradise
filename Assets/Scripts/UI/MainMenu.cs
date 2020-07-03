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

    // Start the game
    public void StartGame()
    {
        SceneManager.LoadScene(levelToLoad);
    }



    // End the program
    public void ExitGame()
    {
        Application.Quit();
    }



    // Prompt player if he is sure he wants to delete PlayerPrefs
    public void DeleteSave()
    {
        deletePanel.SetActive(true);
    }



    // Cancel delete PlayerPrefs command
    public void CancelDelete()
    {
        deletePanel.SetActive(false);
    }



    // Delete character unlocks in PlayerPrefs
    public void ConfirmDelete()
    {
        foreach(CharacterSelector theCharacter in charactersToDelete)
        {
            PlayerPrefs.SetInt(theCharacter.playerToSpawn.name, 0);
        }

        deletePanel.SetActive(false);
    }
}