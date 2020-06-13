using System.Collections;
using System.Collections.Generic;
using UnityEditor.Macros;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Instanzierung der Klasse
    public static PlayerController instance;

    // Variabeln Bewegungslogik
    [Header("Movement")]
    public bool canMove = true;                         // REF ob sich Spieler bewegen darf
    public float moveSpeed;                             // REF Beweguntsgeschwindigkeit
    private float activeMoveSpeed;                      // derzeitige Beweguntsgeschwindigkeit
    private Vector2 moveInput;                          // Bewegungseingabe als Vektor
    public Rigidbody2D theRB;                           // REF Kollisionskörper Spieler
    public Transform gunArm;                            // REF Koordinaten Waffenarm
    public Animator anim;                               // REF Animation
    [HideInInspector] 
    public SpriteRenderer bodySR;                       // REF Body Sprite

    // Variablen Dash-Logik
    [Header("Dashing")]
    public float dashSpeed = 8f;                        // REF Geschwindigkeit Dash
    public float dashLength = 0.5f;                     // REF Dash-Distanz
    public float dashCoolDown = 1f;                     // REF Dauer Dash Cooldown
    public float dashInvincibility = 0.5f;              // REF Dauer Dash Unverletzbarkeit
    private float dashCoolDownCounter;                  // Counter Dash Cooldown
    [HideInInspector]
    public float dashCounter;                           // Counter Dash Unverletzbarkeit

    // Variablen Waffen
    [Header("Weapons")]
    public List<Gun> availableGuns = new List<Gun>();   // REF Liste aller verfügbaren Waffen
    [HideInInspector]
    public int currentGun;                             // Listenindex der aktuellen Waffe


    // Wie Start(), nur davor
    public void Awake()
    {
        instance = this;

        // Spielerobjekt beim Laden einer neuen Szene nicht zerstören
        DontDestroyOnLoad(gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        // Standardgeschwindigkeit setzen
        activeMoveSpeed = moveSpeed;

        // Waffeninformation in UI setzen
        UpdateGunUI();
    }


    // Update is called once per frame
    void Update()
    {
        // Nur wenn sich spieler bewegen darf und keine Pause besteht
        if (canMove && !LevelManager.instance.ispaused)
        {
            MovePlayer();
            PlayerAim();
            PlayerDash();
            AnimatePlayer();
            SelectNextWeapon();
        }
        else
        {
            StopPlayer();
        }
    }



    //
    //  METHODEN
    //

    // Metode Spieler bewegen
    private void MovePlayer()
    {
        // Bewegungseingabe (W,A,S,D) registreiren, speichern und als Einheitsvektor normalisieren
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();
        theRB.velocity = moveInput * activeMoveSpeed;
    }



    // Methode Spieler stoppen
    private void StopPlayer()
    {
        // Spielergeschwindigkeit auf Null setzen
        theRB.velocity = Vector2.zero;
        anim.SetBool("isMoving", false);
    }



    // Methode Spieler zielen
    private void PlayerAim()
    {
        // Mausposition als Bildschirmkoordinate speichern; Globale Spielerposition in Bildschirmkoordinaten umwandeln
        Vector3 mousePos = Input.mousePosition;
        Vector3 screenPoint = CameraController.instance.mainCamera.WorldToScreenPoint(transform.localPosition);

        // Spieler und Waffe link/rechts zur Mausposition spiegeln, mithilfe negativer Spiegelung des Sprites
        if (mousePos.x < screenPoint.x)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            gunArm.localScale = new Vector3(-1f, -1f, 1f);
        }
        else
        {
            transform.localScale = Vector3.one;
            gunArm.localScale = Vector3.one;
        }

        // Waffenrotation von Maus- und Spielerposition als Winkel ableiten und als Quaternionangabe übergeben
        Vector2 offset = new Vector2(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y);
        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        gunArm.rotation = Quaternion.Euler(0, 0, angle);
    }



    // Methode Spieler dashen
    private void PlayerDash()
    {
        // Dash beim Leertastendruck
        if (Input.GetKeyDown(KeyCode.Space) && dashCoolDownCounter <= 0 && dashCounter <= 0)
        {
            activeMoveSpeed = dashSpeed;
            dashCounter = dashLength;
            anim.SetTrigger("dash");

            // Unverletzbar machen und sound spielen
            PlayerHealthController.instance.MakeInvincible(dashInvincibility);
            AudioManager.instance.PlaySFX(8);
        }

        // Counter bis zum Dash-Ende
        if (dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;
            if (dashCounter <= 0)
            {
                activeMoveSpeed = moveSpeed;
                dashCoolDownCounter = dashCoolDown;
            }
        }

        // Counter bis Dash wieder verfügbar ist
        if (dashCoolDownCounter > 0)
        {
            dashCoolDownCounter -= Time.deltaTime;
        }
    }



    // Methode Spieler Animieren
    private void AnimatePlayer()
    {
        // Animations-switch für Stillstand und Bewegung
        if (moveInput != Vector2.zero)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
    }



    // Methode Waffen austauschen
    public void SelectNextWeapon()
    {
        if (Input.GetKeyDown(KeyCode.F) && availableGuns.Count > 0)
        {
            currentGun++;

            if (currentGun >= availableGuns.Count)
            {
                currentGun = 0;
            }

            ActivateGun();
        }
    }



    // Methode nächste Waffe auswählen
    public void ActivateGun()
    {
        // Alle Waffen in der Liste deaktivieren
        foreach (Gun theGun in availableGuns)
        {
            theGun.gameObject.SetActive(false);
        }

        // Waffe aktivieren
        availableGuns[currentGun].gameObject.SetActive(true);

        UpdateGunUI();
    }



    // Methode Waffe in UI aktualisieren
    public void UpdateGunUI()
    {
        UIController.instance.currentGun.sprite = availableGuns[currentGun].gunUI;
        UIController.instance.currentGunText.text = availableGuns[currentGun].weaponName;
    }
}