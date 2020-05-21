using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Instanzierung der Klasse
    public static PlayerController instance;


    // Variabeldeklaration für den gesamten Class-Scope
    public float moveSpeed;         // REF Beweguntsgeschwindigkeit
    private Vector2 moveInput;      // Bewegungseingabe als Vektor

    public Rigidbody2D theRB;       // REF Kollisionskörper Spieler

    public Transform gunArm;        // REF Koordinaten Waffe
    private Camera theCam;          // Var der Kamera

    public Animator anim;           // REF Animation

    public GameObject bulletToFire; // REF Kugelobjekt
    public Transform firePoint;     // REF Ort der Kugelerstellung

    public float timeBetweenShots;  // REF Feuerrate
    private float shotCounter;      // Countdown bis zur nächsten Kugel

    public SpriteRenderer bodySR;   // Ref Body Sprite

    // Wie Start(), nur davor
    public void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        // Kameraobjekt abspeichern, damit dies nicht jedes Frame im Gesampten Projekt gesucht wird
        theCam = Camera.main;
    }


    // Update is called once per frame
    void Update()
    {
        // Bewegungseingabe (W,A,S,D) registreiren, speichern und als Einheitsvektor normalisieren
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();
        theRB.velocity = moveInput * moveSpeed;


        // Mausposition als Bildschirmkoordinate speichern; Globale Spielerposition in Bildschirmkoordinaten umwandeln
        Vector3 mousePos = Input.mousePosition;
        Vector3 screenPoint = theCam.WorldToScreenPoint(transform.localPosition);


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


        // Kugel einzeln per Mausdruck feuern - Instantiate (welches Prefab?, wo soll erstellt werden?, mit welcher Drehung?)
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(bulletToFire, firePoint.position, firePoint.rotation);
            shotCounter = timeBetweenShots;
        }

        // Kugel dauernd per gehaltenem Mausdruck feuern
        if (Input.GetMouseButton(0))
        {
            shotCounter -= Time.deltaTime;
            
            if (shotCounter <= 0)
            {
                Instantiate(bulletToFire, firePoint.position, firePoint.rotation);
                shotCounter = timeBetweenShots;
            }
        }


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
}