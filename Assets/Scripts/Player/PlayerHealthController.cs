using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    // Instancing the class
    public static PlayerHealthController instance;

    // Variables Hitpoints
    public int currentHealth;                   // REF current hitpoints
    public int maxHealth;                       // REF maximum hitpoints

    // Variabeln Unverletzbarkeit
    public float damageInvinceDuration = 1f;    // REF duration of invincibility
    private float invinceCount;                 // Counter until invincibility's end


    // Before Start()
    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        // Set player hitpoints according to CharacterTracker's values
        maxHealth = CharacterTracker.instance.maxHealth;
        currentHealth = CharacterTracker.instance.currentHealth;

        UIController.instance.healthSlider.maxValue = maxHealth;
        updateHealthUI();
    }


    // Update is called once per frame
    void Update()
    {
        MakePlayerInvincible();
    }



    //
    //  METHODS
    //

    // Make player invincible and transparent
    private void MakePlayerInvincible()
    {
        if (invinceCount > 0)
        {
            invinceCount -= Time.deltaTime;

            if (invinceCount <= 0)
            {
                SetBodyAlpha(1f);
            }
        }
    }



    // Damage and kill player
    public void DamagePlayer()
    {
        // Inflict damage if not invincible
        if (invinceCount <= 0)
        {      
            // After inflicted damage, start invincibility counter
            currentHealth--;
            invinceCount = damageInvinceDuration;
            AudioManager.instance.PlaySFX(11);

            // Make player body sprite transparent
            SetBodyAlpha(0.5f);

            // Check if player should die
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



    // Update hitpoints in the UI
    private void updateHealthUI()
    {
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }



    // Toggle player body sprite transparency
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



    // Make player invincible
    public void MakeInvincible(float length)
    {
        invinceCount = length;
        SetBodyAlpha(0.5f);
    }



    // Heal player
    public void HealPlayer(int healAmount)
    {
        currentHealth += healAmount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        updateHealthUI();
    }



    // Extend player maximum hitpoints
    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth = maxHealth;
        UIController.instance.healthSlider.maxValue = maxHealth;
        updateHealthUI();
    }
}