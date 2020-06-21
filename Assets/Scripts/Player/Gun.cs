using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // Variabels shooting
    [Header("Shooting")]
    public GameObject bulletToFire;         // REF bullet game object
    public Transform firePoint;             // REF origin point of the bullet
    public float timeBetweenShots;          // REF rate of fire
    private float shotCounter;              // Countdown until the next bullet

    // Variabels weapon type
    [Header("Weapon Type")]
    public string weaponName;               // REF weapon name
    public Sprite gunUI;                    // REF gun sprite on the UI
    public int itemCost;                    // REF cost of the item in the shop
    public Sprite gunShopSprite;            // REF gun sprite on the shop floor


    // Update is called once per frame
    void Update()
    {
        if (PlayerController.instance.canMove && !LevelManager.instance.ispaused)
        {
            GunShoot();
        }
    }



    //
    // METHODS
    //

    // Shoot gun
    private void GunShoot()
    {
        // Shoot bullets each mouse click, or continue shooting them if mouse button is held down
        if (shotCounter > 0)
        {
            shotCounter -= Time.deltaTime;
        }
        else
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
            {
                Instantiate(bulletToFire, firePoint.position, firePoint.rotation);
                shotCounter = timeBetweenShots;
                AudioManager.instance.PlaySFX(12);
            }
        }
    }
}
