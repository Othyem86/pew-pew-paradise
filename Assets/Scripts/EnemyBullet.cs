using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed;             // REF Geschwindigkeit Kugel
    private Vector3 direction;      // Flugrichtung Kugel

    // Start is called before the first frame update
    void Start()
    {
        direction = PlayerController.instance.transform.position - transform.position;
        direction.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        // Richtung und Geschwindigkeit ohne RigidBody definieren
        transform.position += direction * speed * Time.deltaTime;
    }


    // Funktion der Ereignisse Kollision Kugel bei Trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Nur beim Spieler die Kalkulation ausführen
        if (other.tag == "Player")
        {
            PlayerHealthController.instance.DamagePlayer();
        }

        Destroy(gameObject);
        AudioManager.instance.PlaySFX(4);
    }



    // Kugel ausserhalb des Bidlschirms zerstören
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
