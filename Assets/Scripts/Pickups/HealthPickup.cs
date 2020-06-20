using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    // Variables health pack
    public int healAmount = 1;                  // REF healing abount
    public float waitToBeCollected = 0.5f;      // Delay duration until it can be picked up


    // Update is called once per frame
    void Update()
    {
        // Wait a while before health pack is collectible by player
        if (waitToBeCollected > 0)
        {
            waitToBeCollected -= Time.deltaTime;
        }
    }



    // Heal player when he collides with health pack
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && waitToBeCollected <= 0)
        {
            PlayerHealthController.instance.HealPlayer(healAmount);
            Destroy(gameObject);
            AudioManager.instance.PlaySFX(7);
        }
    }
}