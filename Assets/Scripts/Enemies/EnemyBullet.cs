using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    // Variables enemy bullet
    public float speed;             // REF enemy bullet speed
    private Vector3 direction;      // bullet direction vector


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



    //
    //  METHODS
    //

    // Method enemy bullet collision events
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerHealthController.instance.DamagePlayer();
        }

        Destroy(gameObject);
        AudioManager.instance.PlaySFX(4);
    }



    // Method destroy enemy bullet when not visible on screen
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
