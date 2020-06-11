using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    // Variabeln Shop
    public GameObject buyMessage;       // REF Kauftext
    private bool inBuyZone;             // Ob Spieler in Kauffläche steht

    // Variabeln Kaufgegenstand
    [Header("Item parameters")]
    public bool isHealthRestore;        // REF Ob Kaufgegenstand Heilen ist
    public bool isHealthUpgrade;        // REF Ob Kaufgegenstand Upgrade Hitpoints ist
    public bool isWeapon;               // REF Ob Kaufgegenstand Waffe ist
    public int itemCost;                // REF Preis des Kaufgegenstands
    public int upgradeHealthAmount;     // REF Wieviel die Hitpoints erweitert

    // Variabeln Waffenkauf
    [Header("Weapons to buy")]
    public Gun[] potentialGuns;         // REF Lise der zu verkaufende Waffen
    private Gun theGun;                 // die einzelne Waffe die verkauft wird
    public SpriteRenderer gunSprite;    // Sprite der einzelnen Waffe die verkauft wird
    public Text infoText;               // Preisinformatien der Waffe die verkauft wird


    // Start is called before the first frame update
    void Start()
    {
        if (isWeapon)
        {
            int selectedGun = Random.Range(0, potentialGuns.Length);
            theGun = potentialGuns[selectedGun];

            gunSprite.sprite = theGun.gunShopSprite;
            itemCost = theGun.itemCost;
            infoText.text = theGun.weaponName + "\n- " + theGun.itemCost + " - ";
        }
    }


    // Update is called once per frame
    void Update()
    {
        BuyItem();
    }



    //
    //  METHODEN
    //

    // Methode Kauftext deaktivieren wenn Spieler reingeht
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            buyMessage.SetActive(true);
            inBuyZone = true;
        }
    }



    // Methode Kauftext deaktivieren wenn Spieler rausgeht
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            buyMessage.SetActive(false);
            inBuyZone = false;
        }
    }



    // Methode Gegenstand kaufen
    private void BuyItem()
    {
        if (inBuyZone)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (LevelManager.instance.currentCoins >= itemCost)
                {
                    LevelManager.instance.SpendCoins(itemCost);

                    // Spieler heilen
                    if (isHealthRestore)
                    {
                        PlayerHealthController.instance.HealPlayer(PlayerHealthController.instance.maxHealth);
                    }

                    // Spieler Hitpoints erweitern
                    if (isHealthUpgrade)
                    {
                        PlayerHealthController.instance.IncreaseMaxHealth(upgradeHealthAmount);
                    }

                    // Waffe kaufen
                    if (isWeapon)
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

                    // Kaufgegenstand deaktivieren
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
