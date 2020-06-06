using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Variabeln Bewegung
    public float moveSpeed;                 // REF Beweguntsgeschwindigkeit
    public Rigidbody2D theRB;               // REF Kollisionskörper Spieler
    private Vector3 moveDirection;          // Bewegungsrichtung des Gegners
    public Animator anim;                   // REF Animation

    // Variabeln Verfolgen
    public bool shouldChasePlayer;          // REF ob Gegner Spieler verfolgen soll
    public float rangeToChasePlayer;        // REF Verfolgungsradius

    // Variabeln Fliehen
    public bool shouldRunAway;              // REF ob Gegner vor Spieler fliehen soll
    public float rangeToRunAway;            // REF Fluchtradius

    // Variabeln Zufallbewegung
    public bool shouldWander;               // REF ob Gegner sich zufällig bewegen soll
    public float wanderLength;              // REF wie lange Gegner sich bewegen soll
    public float pauseLength;               // REF wie lange Gegner pausen soll
    private float wanderCounter;            //  
    private float pauseCounter;             // 
    private Vector3 wanderDirection;        // Bewegungsrichtung

    // Variabeln Hitpoints
    public int health = 150;                // REF Hitpoints
    public GameObject[] deathSplatters;     // REF Todanimation
    public GameObject hitEffect;            // REF Treffereffekt

    // Variabeln Schiessen
    public bool shouldShoot;                // Ob es schiessen soll
    public GameObject bullet;               // REF Kugel
    public Transform firePoint;             // REF Kugelursprung
    public float fireRate;                  // REF Schussfrequenz
    private float fireCounter;              // Countdown
    public float shootRange;                // REF Schussreichweite
    public SpriteRenderer enemyBody;        // REF Renderer Körper Gegner


    // Start is called before the first frame update
    void Start()
    {
        // Am Start des Raumes, Zufällige Pausenzeit generieren
        if (shouldWander)
        {
            pauseCounter = Random.Range(pauseLength * 0.75f, pauseLength * 1.25f);
        }
    }


    // Update is called once per frame
    void Update()
    {
        // Schiessen und Bewegen je nach Gegnertyp
        if (enemyBody.isVisible && PlayerController.instance.gameObject.activeInHierarchy)
        {
            moveDirection = Vector3.zero;

            EnemyShoot();
            EnemyChase();
            EnemyFlee();

            // Bewegungsrichtung normalisieren, Geschiwindigkeit Kollisionskörper berechnen
            moveDirection.Normalize();
            theRB.velocity = moveDirection * moveSpeed;
        }
        else
        {
            theRB.velocity = Vector3.zero;
        }

        // Gegner animieren
        AnimateEnemy();
    }



    //
    //  FUNKTIONEN
    //

    // Funktion Gegner Schaden
    public void DamageEnemy(int damage)
    {
        // Schaden vom HP abziehen und Shadenanimation generieren
        health -= damage;
        AudioManager.instance.PlaySFX(2);
        Instantiate(hitEffect, transform.position, transform.rotation);


        // Zerstörung des Gegners wenn Hitpoints Null sind und Reste generieren
        if (health <= 0)
        {
            Destroy(gameObject);
            AudioManager.instance.PlaySFX(1);

            // Spur hinterlassen
            int selectedSplatter = Random.Range(0, deathSplatters.Length);
            int rotation = Random.Range(0, 360);
            Instantiate(deathSplatters[selectedSplatter], transform.position, Quaternion.Euler(0f, 0f, rotation));
        }
    }


    // Funktion Verfolgen
    private void EnemyChase()
    {
        // Spieler Verfolgen wenn Spieler in Verfolgungsradius
        if (shouldChasePlayer && Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeToChasePlayer)
        {
            moveDirection = PlayerController.instance.transform.position - transform.position;
        }
        else if (shouldWander)
        {
            if (wanderCounter > 0)
            {
                wanderCounter -= Time.deltaTime;

                moveDirection = wanderDirection;

                if (wanderCounter <= 0)
                {
                    pauseCounter = Random.Range(pauseLength * 0.75f, pauseLength * 1.25f);
                }
            }
           

            if (pauseCounter > 0) 
            {
                pauseCounter -= Time.deltaTime;

                if (pauseCounter <= 0)
                {
                    wanderCounter = Random.Range(wanderLength * 0.75f, wanderLength * 1.25f);

                    wanderDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
                }
            }
        }
    }


    // Funktion Fliehen
    private void EnemyFlee()
    {
        // Fliehen wenn Spieler in Fluchtradius
        if (shouldRunAway && Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeToRunAway)
        {
            moveDirection = transform.position - PlayerController.instance.transform.position;
        }
    }


    // Funktion Schiessen
    private void EnemyShoot()
    {
        // Soll schiesse wenn Spieler in Reicheweite
        if (shouldShoot && Vector3.Distance(transform.position, PlayerController.instance.transform.position) < shootRange)
        {
            fireCounter -= Time.deltaTime;

            if (fireCounter <= 0)
            {
                fireCounter = fireRate;
                Instantiate(bullet, firePoint.position, firePoint.rotation);
                AudioManager.instance.PlaySFX(13);
            }
        }
    }


    // Funktion Gegner animieren
    private void AnimateEnemy()
    {
        // Animations-switch für Stillstand und Bewegung
        if (moveDirection != Vector3.zero)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
    }
}
