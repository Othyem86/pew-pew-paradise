using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // Variabeln Schusslogik
    [Header("Shooting")]
    public GameObject bulletToFire;         // REF Kugelobjekt
    public Transform firePoint;             // REF Ort der Kugelerstellung
    public float timeBetweenShots;          // REF Feuerrate
    private float shotCounter;              // Countdown bis zur nächsten Kugel

    // Variabeln Waffentyp
    [Header("Weapon Type")]
    public string weaponName;               // REF Name der Waffe
    public Sprite gunUI;                    // REF Bild der Waffe für UI
    public int itemCost;                    // REF Preis der Waffe im Shop
    public Sprite gunShopSprite;            //


    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        if (PlayerController.instance.canMove && !LevelManager.instance.ispaused)
        {
            GunShoot();
        }
    }



    //
    // METHODEN
    //

    // Methode schiessen
    private void GunShoot()
    {
        // Kugel einzeln per Mausdruck oder dauernd per gehaltenem Mausdruck feuern
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
