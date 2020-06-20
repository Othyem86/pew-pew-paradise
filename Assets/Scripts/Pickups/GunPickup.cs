using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{
    // Variables weapon pickup
    public Gun theGun;
    public float waitToBeCollected = 0.5f;      // Delay duration until it can be picked up


    // Update is called once per frame
    void Update()
    {
        // Wait a while before weapon is collectible by player
        if (waitToBeCollected > 0)
        {
            waitToBeCollected -= Time.deltaTime;
        }
    }



    // Pick up weapon if player collides with it
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && waitToBeCollected <= 0)
        {
            bool hasGun = false;

            // Check if player already has weapon
            foreach (Gun playerOwnedGun in PlayerController.instance.availableGuns)
            {
                if (theGun.weaponName == playerOwnedGun.weaponName)
                {
                    hasGun = true;
                }
            }

            // Add weapon if player doesn't have it aready
            if (!hasGun)
            {
                // Instantiate new weapon as child of gunArm game object
                Gun gunClone = Instantiate(theGun);
                gunClone.transform.parent = PlayerController.instance.gunArm;
                gunClone.transform.position = PlayerController.instance.gunArm.position;
                gunClone.transform.localRotation = Quaternion.Euler(Vector3.zero);
                gunClone.transform.localScale = Vector3.one;

                // Add new weapon to player's available weapons array, and set it as active weapon
                PlayerController.instance.availableGuns.Add(gunClone);
                PlayerController.instance.currentGun = PlayerController.instance.availableGuns.Count - 1;
                PlayerController.instance.ActivateGun();
            }

            Destroy(gameObject);
            AudioManager.instance.PlaySFX(7);
        }
    }
}
