using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    // Variabeln Wert
    [Header("Coin Value")]
    public int coinValue = 1;           // Wert der Münze
    public float waitToBeCollected;     // Zeit bis es aktiviert werden kann


    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        // Abwarten je nach waitToBeCollected bis die Münze aktiviert werden kann
        if (waitToBeCollected > 0)
        {
            waitToBeCollected -= Time.deltaTime;
        }
    }


    // Methode Geld aufheben wenn Spieler mit Münze kollidiert
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && waitToBeCollected <= 0)
        {
            LevelManager.instance.GetCoins(coinValue);
            Destroy(gameObject);
            AudioManager.instance.PlaySFX(5);
        }
    }
}