using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    // Variabeln Spielerkugel
    public float speed = 7.5f;          // REF Kugelgeschwindigkeit
    public Rigidbody2D theRB;           // REF Kollisionskörper
    public GameObject impactEffect;     // REF Partikelefekt bei Kollisionen
    public int bulletDamage = 50;       // REF Schadenswert der Kugel


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
        AudioManager.instance.PlaySFX(4);
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
