using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    // Variabeldeklaration
    public float speed = 7.5f;            // Kugelgeschwindigkeit
    public Rigidbody2D theRB;           // Kollisionskörper für Kugel

    public GameObject impactEffect;     // Partikelefekt bei Kollisionen

    public int bulletDamage = 50;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Nach rechts, relativ zur eigenen Orientierung bewegen
        theRB.velocity = transform.right * speed;
    }

    // Kugelobjekt zerstören wenn es kollidiert und Partikelsystem initialisieren
    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
        Instantiate(impactEffect, transform.position, transform.rotation);

        if(other.tag == "Enemy")
        {
            other.GetComponent<EnemyController>().DamageEnemy(bulletDamage);
        }  
    }

    // Kugelobjekt zerstören wenn es nicht mehr Sichtbar ist
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
