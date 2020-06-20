using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    // Variables ccurrency value
    [Header("Coin Value")]
    public int coinValue = 1;           // Currency value
    public float waitToBeCollected;     // Delay duration until it can be picked up


    // Update is called once per frame
    void Update()
    {
        // Wait a while before coin is collectible by player
        if (waitToBeCollected > 0)
        {
            waitToBeCollected -= Time.deltaTime;
        }
    }


    // Pick up coin if player collides with it
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