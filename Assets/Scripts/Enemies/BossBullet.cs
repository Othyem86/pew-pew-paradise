using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    // Variabels boss bullet
    public float speed;             // REF boss bullet speed
    private Vector3 direction;      // boss bullet direction


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
