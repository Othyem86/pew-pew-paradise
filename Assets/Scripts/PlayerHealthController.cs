using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    // Instanzierung der Klasse
    public static PlayerHealthController instance;

    // Variabeln Hitpoints
    public int currentHealth;               // REF aktuelle Hitpoints
    public int maxHealth;                   // REF maximale Hitpoints

    // Variabeln Unverletzbarkeit
    public float damageInvinceLength = 1f;  // REF Zeit unverletzbar
    private float invinceCount;             // Countdown



    // Wie Start(), nur davor
    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        UIController.instance.healthSlider.maxValue = maxHealth;
        updateHealthUI();
    }


    // Update is called once per frame
    void Update()
    {
        if (invinceCount > 0)
        {
            invinceCount -= Time.deltaTime;
            
            // Wenn nicht mehr unverletzbar, dann Spielertransparenz ausschalten
            if (invinceCount <= 0)
            {
                SetBodyAlpha(1f);
            }
        }
    }


    // Funktion Schadenanrichtung an Spieler und dessen Tod
    public void DamagePlayer()
    {
        // Schadenanrichtung wenn NICHT Unverletzbar
        if (invinceCount <= 0)
        {      
            // Nach schaden, startet den Unverletzbar-Countdown
            currentHealth--;
            invinceCount = damageInvinceLength;
            AudioManager.instance.PlaySFX(11);

            // Schadeneffekt BodySprite - Transparenz einschalten
            SetBodyAlpha(0.5f);

            // Check ob Spieler Tod ist
            if (currentHealth <= 0)
            {
                PlayerController.instance.gameObject.SetActive(false);
                UIController.instance.deathScreen.SetActive(true);
                AudioManager.instance.PlaySFX(9);
                AudioManager.instance.PlayGameOver();
            }

            updateHealthUI();
        }
    }


    // Funktion zur Aktualisierung des Hitpoint-UIs 
    private void updateHealthUI()
    {
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }


    // Funktion Schadeneffekt BodySprite: Transparenz ein-/ausschalten
    private void SetBodyAlpha(float alphaValue)
    {
        PlayerController.instance.bodySR.color = new Color
        (
            PlayerController.instance.bodySR.color.r,
            PlayerController.instance.bodySR.color.b,
            PlayerController.instance.bodySR.color.g,
            alphaValue
        ); 
    }


    // Funktion unverletzbar zu werden
    public void MakeInvincible(float length)
    {
        invinceCount = length;
        SetBodyAlpha(0.5f);
    }


    // Funktion zur Heilung des Spielers
    public void HealPlayer(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        updateHealthUI();
    }
}
