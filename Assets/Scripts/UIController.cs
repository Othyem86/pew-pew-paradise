using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // Instancing the class
    public static UIController instance;

    // Variabels UI
    [Header("HUD Parameters")]
    public Slider healthSlider;         // REF HP-Slider
    public Text healthText;             // REF Text HP-Sliders
    public Text coinText;               // REF Text des currency counter
    public Image currentGun;            // REF UI-sprite of active weapon
    public Text currentGunText;         // REF UI-text of active weapon

    // Variabels scene transition
    [Header("Scene Transition")]
    public Image fadeScreen;            // REF Übergangsbild
    public float fadeSpeed;             // REF Übergangsgeschwindigkeit
    private bool fadeToBlack;           // REF ob Übergang zu Schwarz
    private bool fadeOutBlack;          // REF ob Übergang aus Schwarz

    // Variabels death screen
    [Header("Death Screen")]
    public GameObject deathScreen;      // REF Death Screen Objekt
    public string newGameScene;         // REF Szene Nees Spiel
    public string mainMenuScene;        // REF Szene Hauptmenü

    // Variabels pause screen und minimap
    [Header("Screens")]
    public GameObject pauseMenu;        // REF Pause Screen Objekt
    public GameObject mapDisplay;       // REF Minimap Objekt
    public GameObject bigMapText;       // REF Text Karte Objekt
    public GameObject miniMapText;      // REF Text Minimap Objekt

    // Variables boss
    [Header("Boss UI")]
    public Slider bossHealthBar;        // REF Boss Health Slider


    // Before Start()
    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        PlayerController.instance.UpdateGunUI();

        // Am anfang Übergang von Schwarz
        fadeOutBlack = true;
        fadeToBlack = false;
    }


    // Update is called once per frame
    void Update()
    {
        // Übergang aus Schwarz
        if (fadeOutBlack)
        {
            FadeScreen(0f);
            if (fadeScreen.color.a == 0f)
            {
                fadeOutBlack = false;
            }
        }


        // Übergang zu Schwarz
        if (fadeToBlack)
        {
            FadeScreen(1f);
            if (fadeScreen.color.a == 1f)
            {
                fadeToBlack = false;
            }
        }
    }


    // Funktion Start Übergang zu Schwarz
    public void StartFadeToBlack()
    {
        fadeToBlack = true;
        fadeOutBlack = false;
    }


    // Funktion Übergang Tansparenz
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


    // Funktion neues Spiel und Zeitverlauf auf 100% setzen
    public void NewGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(newGameScene);
        Destroy(PlayerController.instance.gameObject);
    }


    // Funktion zurück zum Hauptmenü und Zeitverlauf auf 100% setzen
    public void MainMenu() 
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(mainMenuScene);
        Destroy(PlayerController.instance.gameObject);
    }


    // Funktion Pause beenden
    public void Resume()
    {
        LevelManager.instance.PauseUnpause();
    }
}