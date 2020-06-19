using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    // Variabels boss bullet
    public float speed;             // REF bullet speed
    private Vector3 direction;      // bullet direction


    // Start is called before the first frame update
    void Start()
    {
        direction = transform.right;
    }


    // Update is called once per frame
    void Update()
    {
        // define the direction and the speed
        transform.position += direction * speed * Time.deltaTime;

        if (!BossController.instance.gameObject.activeInHierarchy)
        {
            Destroy(gameObject);
        }
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
