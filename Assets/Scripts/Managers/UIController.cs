using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // Instancing the class
    public static UIController instance;

    // Variables UI
    [Header("HUD Parameters")]
    public Slider healthSlider;         // REF hitpoint slider
    public Text healthText;             // REF text HP-Sliders
    public Text coinText;               // REF text currency counter
    public Image currentGun;            // REF UI-sprite of active weapon
    public Text currentGunText;         // REF UI-text of active weapon

    // Variables scene transition
    [Header("Scene Transition")]
    public Image fadeScreen;            // REF image of the fade in/out effect
    public float fadeSpeed;             // REF speed fade in/out
    private bool fadeToBlack;           // REF if fade to black
    private bool fadeOutBlack;          // REF if fade out black

    // Variables death screen
    [Header("Death Screen")]
    public GameObject deathScreen;      // REF death screen object
    public string newGameScene;         // REF scene new game
    public string mainMenuScene;        // REF scene main menu

    // Variables pause screen and minimap
    [Header("Screens")]
    public GameObject pauseMenu;        // REF pause screen object
    public GameObject mapDisplay;       // REF minimap object
    public GameObject bigMapText;       // REF big map text object
    public GameObject miniMapText;      // REF minimap text object

    // Variables boss
    [Header("Boss UI")]
    public Slider bossHealthBar;        // REF boss health slider


    // Before Start()
    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        PlayerController.instance.UpdateGunUI();

        // fade out black
        fadeOutBlack = true;
        fadeToBlack = false;
    }


    // Update is called once per frame
    void Update()
    {
        // Fade out black
        if (fadeOutBlack)
        {
            FadeScreen(0f);
            if (fadeScreen.color.a == 0f)
            {
                fadeOutBlack = false;
            }
        }


        // Fade to black
        if (fadeToBlack)
        {
            FadeScreen(1f);
            if (fadeScreen.color.a == 1f)
            {
                fadeToBlack = false;
            }
        }
    }



    //
    // METHODS
    //

    // Method start fade to black
    public void StartFadeToBlack()
    {
        fadeToBlack = true;
        fadeOutBlack = false;
    }



    // Method fade in/out fadescreen
    private void FadeScreen(float alpha)
    {
        fadeScreen.color = new Color
            (
                fadeScreen.color.r,
                fadeScreen.color.g,
                fadeScreen.color.b,
                Mathf.MoveTowards
                (fadeScreen.color.a, alpha, fadeSpeed * Time.deltaTime)
            );
    }



    // Method new game and set game speed to 1
    public void NewGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(newGameScene);
        Destroy(PlayerController.instance.gameObject);
    }



    // Method to main menu and set game speed to 1
    public void MainMenu() 
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(mainMenuScene);
        Destroy(PlayerController.instance.gameObject);
    }



    // Method end pause
    public void Resume()
    {
        LevelManager.instance.PauseUnpause();
    }
}