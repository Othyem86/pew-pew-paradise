using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Variabeln Bewegung
    public float moveSpeed;                 // REF Beweguntsgeschwindigkeit
    public Rigidbody2D theRB;               // REF Kollisionskörper Spieler
    public float rangeToChasePlayer;        // REF minimale Distanz für Verfolgung
    private Vector3 moveDirection;          // Bewegungsrichtung des Gegners
    public Animator anim;                   // REF Animation

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
        
    }


    // Update is called once per frame
    void Update()
    {
        // Schiessen nur wenn Körper des Gegners auf dem Bildschirm sichtbar ist, bzw. in der Welt existiert
        if (enemyBody.isVisible && PlayerController.instance.gameObject.activeInHierarchy)
        {
            // Wenn Abstand kleiner als minimale Distanz, dann wird der der Vektor3 zum Spieler generiert, sonst Nullvektor
            if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeToChasePlayer)
            {
                moveDirection = PlayerController.instance.transform.position - transform.position;
            }
            else
            {
                moveDirection = Vector3.zero;
            }


            // Vektor in Einheitsvektor normalisieren
            moveDirection.Normalize();


            // Geschiwindigkeit des Kollisionskörper berechnen
            theRB.velocity = moveDirection * moveSpeed;


            // Soll nur schiessen wenn der Spieler in die Schussreichweite steht
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
        else
        {
            theRB.velocity = Vector3.zero;
        }


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



    // Schaden- und Todanimation des Gegners
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

            int selectedSplatter = Random.Range(0, deathSplatters.Length);
            int rotation = Random.Range(0, 360);
            Instantiate(deathSplatters[selectedSplatter], transform.position, Quaternion.Euler(0f, 0f, rotation));
        }
    }
}
