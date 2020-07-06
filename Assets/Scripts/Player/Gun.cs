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
    public bool doubleMirror;               // REF if bullet should be doubly mirrored
    public int soundSFX;                    // REF Bullet soundeffect

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
                GameObject bullet = Instantiate(bulletToFire, firePoint.position, firePoint.rotation);
                shotCounter = timeBetweenShots;
                AudioManager.instance.PlaySFX(soundSFX);

                // Mirror player and weapon left/right, towars mouse position
                if (PlayerController.instance.mousePos.x < PlayerController.instance.screenPoint.x)
                {
                    if (doubleMirror)
                    {
                        bullet.transform.localScale = new Vector3(-1f, -1f, 1f);
                    }
                    else
                    {
                        bullet.transform.localScale = new Vector3(1f, -1f, 1f);
                    }
                }
            }
        }
    }
}
