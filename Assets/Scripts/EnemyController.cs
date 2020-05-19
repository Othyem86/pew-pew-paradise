using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Variabeldeklaration für den gesamten Class-Scope
    public float moveSpeed;             // Beweguntsgeschwindigkeit
    public Rigidbody2D theRB;           // Kollisionskörper für Spieler

    public float rangeToChasePlayer;    // minimale Distanz für Verfolgung
    private Vector3 moveDirection;      // Bewegungsrichtung des Gegners

    public Animator anim;               // Animation

    public int health = 150;


    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
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

    // 
    public void DamageEnemy(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
