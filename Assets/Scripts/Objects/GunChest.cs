using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunChest : MonoBehaviour
{
    // Variables gun chest
    [Header("General")]
    public SpriteRenderer theSR;                    // REF SpriteRenderer
    public Sprite chestOpen;                        // REF Sprite for the opened chest
    public GameObject notification;                 // REF open message

    // Variables drop weapon
    [Header("Drops")]
    public Transform spawnPoint;                    // REF generation point of the drop
    public GunPickup[] potentialGunDrops;           // REF array of all the possible drops
    public float scaleSpeed = 3f;                   // REF speend open animation
    private bool canOpen;                           // If the player can open the chest
    private bool isOpen;                            // If the player has opened the chest


    // Update is called once per frame
    void Update()
    {
        OpenChest();
    }



    //
    // METHODS
    //

    // Open chest
    private void OpenChest()
    {
        if(canOpen && !isOpen)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                int gunDropSelect = Random.Range(0, potentialGunDrops.Length);

                Instantiate(potentialGunDrops[gunDropSelect], spawnPoint.position, spawnPoint.rotation);

                theSR.sprite = chestOpen;
                notification.SetActive(false);
                transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                isOpen = true;
            }
        }

        // Short animation
        if (isOpen)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one, Time.deltaTime * scaleSpeed);
        }
    }



    // Player near weapon chest
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !isOpen)
        {
            notification.SetActive(true);
            canOpen = true;
        }
    }



    // Player not near weapon chest
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            notification.SetActive(false);
            canOpen = false;
        }
    }
}
