using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // Instanzierung der Klasse
    public static UIController instance;

    // Variabeln UI
    public Slider healthSlider;         // REF HP-Slider
    public Text healthText;             // REF Text des HP-Sliders

    // Variabeln Szenenübergang
    public GameObject deathScreen;      // REF Death Screen
    public Image fadeScreen;            // REF Übergangsbild
    public float fadeSpeed;             // REF Übergangsgeschwindigkeit
    private bool fadeToBlack;           // REF ob Übergang zu Schwarz
    private bool fadeOutBlack;          // REF ob Übergang aus Schwarz


    // Wie Start(), nur davor
    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
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
}