using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{
    // Variabeln Waffenpickup
    public Gun theGun;
    public float waitToBeCollected = 0.5f;      // REF Zeit bis es aktiviert werden kann


    // Update is called once per frame
    void Update()
    {
        // Abwarten je nach waitToBeCollected bis die Waffe aktiviert werden kann
        if (waitToBeCollected > 0)
        {
            waitToBeCollected -= Time.deltaTime;
        }
    }



    // Waffe aufheben
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && waitToBeCollected <= 0)
        {
            bool hasGun = false;

            // Checken ob Spieler die Waffe schon hat
            foreach (Gun playerOwnedGun in PlayerController.instance.availableGuns)
            {
                if (theGun.weaponName == playerOwnedGun.weaponName)
                {
                    hasGun = true;
                }
            }

            // Wenn spieler Waffe nicht hat, Waffe hinzufügen
            if (!hasGun)
            {
                // Neue Waffe als Kind der Waffenhand instantieren
                Gun gunClone = Instantiate(theGun);
                gunClone.transform.parent = PlayerController.instance.gunArm;
                gunClone.transform.position = PlayerController.instance.gunArm.position;
                gunClone.transform.localRotation = Quaternion.Euler(Vector3.zero);
                gunClone.transform.localScale = Vector3.one;

                // Neue Waffe zur Spielerwaffenliste hinzufügen und zu ihr wechseln
                PlayerController.instance.availableGuns.Add(gunClone);
                PlayerController.instance.currentGun = PlayerController.instance.availableGuns.Count - 1;
                PlayerController.instance.ActivateGun();
            }

            Destroy(gameObject);
            AudioManager.instance.PlaySFX(7);
        }
    }
}
