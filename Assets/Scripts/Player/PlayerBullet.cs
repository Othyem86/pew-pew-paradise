using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    // Variables player bullet
    public float speed = 7.5f;          // REF bullet speed
    public Rigidbody2D theRB;           // REF bullet rigid body
    public GameObject impactEffect;     // REF partikle effect for the bullet collision
    public int bulletDamage = 50;       // REF damage amount of a bullet hit


    // Update is called once per frame
    void Update()
    {
        // move bullet to the right, relative to it's current orientation
        theRB.velocity = transform.right * speed;
    }



    //
    //  METHODS
    //

    // Destroy bullet on collision and generate particle effect
    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
        AudioManager.instance.PlaySFX(4);
        Instantiate(impactEffect, transform.position, transform.rotation);

        if(other.tag == "Enemy")
        {
            other.GetComponent<EnemyController>().DamageEnemy(bulletDamage);
        }

        if (other.tag == "Boss")
        {
            BossController.instance.TakeDamage(bulletDamage);
            Instantiate(BossController.instance.hitEffect, transform.position, transform.rotation);
        }
    }



    // Destroy the bulllet when it's no longer visible on the screen
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
