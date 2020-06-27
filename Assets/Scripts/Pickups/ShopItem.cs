using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    // Variables Shop
    public GameObject buyMessage;       // REF buy message
    private bool inBuyZone;             // If player is in the buy area

    // Variables Buy health related items
    [Header("Item parameters")]
    public bool isHealthRestore;        // REF if shop item is a healing item
    public bool isHealthUpgrade;        // REF if shop item is a extend health item
    public bool isWeapon;               // REF if shop item is a weapon item
    public int itemCost;                // REF price of item
    public int upgradeHealthAmount;     // REF extend health value

    // Variables Buy Weapon
    [Header("Weapons to buy")]
    public Gun[] potentialGuns;         // REF array of all possible weapons items on sale
    private Gun theGun;                 // the randomly generated weapon item on sale
    public SpriteRenderer gunSprite;    // Sprite of weapon item on sale
    public Text infoText;               // Price info of weapon item on sale


    // Start is called before the first frame update
    void Start()
    {
        DisplayRandomWeapon();
    }


    // Update is called once per frame
    void Update()
    {
        BuyItem();
    }



    //
    //  METHODS
    //

    // Display random weapon
    private void DisplayRandomWeapon()
    {
        if (isWeapon)
        {
            int selectedGun = Random.Range(0, potentialGuns.Length);
            theGun = potentialGuns[selectedGun];

            gunSprite.sprite = theGun.gunShopSprite;
            itemCost = theGun.itemCost;
            infoText.text = theGun.weaponName + "\n- " + theGun.itemCost + "x    - ";
        }
    }



    // Activate buy message when player leaves buy area
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            buyMessage.SetActive(true);
            inBuyZone = true;
        }
    }



    // Activate buy message when player leaves buy area
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            buyMessage.SetActive(false);
            inBuyZone = false;
        }
    }



    // Buy item
    private void BuyItem()
    {
        if (inBuyZone)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (LevelManager.instance.currentCoins >= itemCost)
                {
                    LevelManager.instance.SpendCoins(itemCost);

                    // Heal player
                    if (isHealthRestore)
                    {
                        PlayerHealthController.instance.HealPlayer(PlayerHealthController.instance.maxHealth);
                    }

                    // Extend player health
                    if (isHealthUpgrade)
                    {
                        PlayerHealthController.instance.IncreaseMaxHealth(upgradeHealthAmount);
                    }

                    // Buy weapon
                    if (isWeapon)
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

                    // Deactivate bought object
                    gameObject.SetActive(false);
                    inBuyZone = false;
                    AudioManager.instance.PlaySFX(18);
                }
                else
                {
                    AudioManager.instance.PlaySFX(19);
                }
            }
        }
    }
}
