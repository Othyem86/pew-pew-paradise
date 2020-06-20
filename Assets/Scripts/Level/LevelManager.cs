using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Instancing the class
    public static LevelManager instance;

    // Variables scene transition
    [Header("Scene Transition")]
    public float waitToLoad = 3f;       // REF duration until next scene
    public string nextLevel;            // REF next scene
    public Transform startPoint;        // REF player starting point

    // Variables Spielpause
    [Header("Pause / Unpause")]
    public bool ispaused;               // REF if the game is paused

    // Variables currency system
    [Header("Money Tracker")]
    public int currentCoins;            // REF current currency amount


    // Before Start()
    public void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        // move player to starting point
        PlayerController.instance.transform.position = startPoint.position;
        PlayerController.instance.canMove = true;

        // set currency amount according to values in character tracker
        currentCoins = CharacterTracker.instance.currentCoins;

        // set game speed to 1
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
    //  METHODS
    //

    // Method Coroutine for changing the scene
    public IEnumerator LevelEnd()
    {
        // turn off player movement before scene change
        AudioManager.instance.PlayLevelWin();
        PlayerController.instance.canMove = false;
        UIController.instance.StartFadeToBlack();
        yield return new WaitForSeconds(waitToLoad);

        // save current hitpoints, max hitpoints and currency amount
        CharacterTracker.instance.currentCoins = currentCoins;
        CharacterTracker.instance.currentHealth = PlayerHealthController.instance.currentHealth;
        CharacterTracker.instance.maxHealth = PlayerHealthController.instance.maxHealth;

        // change scene
        SceneManager.LoadScene(nextLevel);
    }



    // Method pause game
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



    // Method get coins
    public void GetCoins(int amount)
    {
        currentCoins += amount;

        // UI Update
        UIController.instance.coinText.text = currentCoins.ToString();
    }
    
    

    // Method spend coins
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