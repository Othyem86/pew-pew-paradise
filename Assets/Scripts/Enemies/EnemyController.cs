using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Variables movement
    [Header("Movement")]
    public float moveSpeed;                 // REF enemy move speed
    public Rigidbody2D theRB;               // REF enemy rigidbody
    private Vector3 moveDirection;          // Enemy direction vector
    public Animator anim;                   // REF enemy animation

    // Variables chase player
    [Header("Chase Player")]
    public bool shouldChasePlayer;          // REF if enemy should chase player
    public float rangeToChasePlayer;        // REF chase range

    // Variables flee from player
    [Header("Run Away")]
    public bool shouldRunAway;              // REF if enemy should run away from player
    public float rangeToRunAway;            // REF run away range

    // Variables Roaming
    [Header("Wander")]
    public bool shouldWander;               // REF if enemy should roam randomly
    public float wanderLength;              // REF duration of roam movement
    public float pauseLength;               // REF duration of pause between movements
    private float wanderCounter;            // Countdown until next movement pause
    private float pauseCounter;             // Countdown until next movement
    private Vector3 wanderDirection;        // Roaming direction vector

    // Variables patrolling
    [Header("Patrolling")]
    public bool shouldPatrol;               // REF if enemy should patrol
    public Transform[] patrolPoints;        // REF array of all patrol waypoints
    private int currentPatrolPoint;         // next waypoint 

    // Variables enemy hitpoints
    [Header("Hitpoints")]
    public int health = 150;                // REF Hitpoints
    public GameObject[] deathSplatters;     // REF array of all possible death splatters
    public GameObject hitEffect;            // REF hit particle effect

    // Variables enemy shooting
    [Header("Shooting")]
    public bool shouldShoot;                // Ob if enemy should shoot
    public GameObject bullet;               // REF bullet
    public Transform firePoint;             // REF bullet origin
    public float fireRate;                  // REF rate of fire
    private float fireCounter;              // Countdown until the next bullet
    public float shootRange;                // REF range of shooting
    public SpriteRenderer enemyBody;        // REF enemy's sprite renderer

    // Variables enemy random drops
    [Header("Drops")]
    public bool shouldDropItem;             // REF if it should drop
    public GameObject[] itemsToDrop;        // REF Array of possible drops
    public float itemDropPercent;           // REF chances of a drop occurring


    // Start is called before the first frame update
    void Start()
    {
        // Random wandar pause at the start of the scene
        if (shouldWander)
        {
            pauseCounter = Random.Range(pauseLength * 0.75f, pauseLength * 1.25f);
        }
    }


    // Update is called once per frame
    void Update()
    {
        // Move and shoot, according to enemy settings
        if (enemyBody.isVisible && PlayerController.instance.gameObject.activeInHierarchy)
        {
            // Move enemy
            moveDirection = Vector3.zero;
            EnemyShoot();
            EnemyMove();

            // Normalize enemy speed
            moveDirection.Normalize();
            theRB.velocity = moveDirection * moveSpeed;
        }
        else
        {
            theRB.velocity = Vector3.zero;
        }

        // Enemy animation
        AnimateEnemy();
    }



    //
    //  METHODs
    //

    // Method damage enemy
    public void DamageEnemy(int damage)
    {
        // Subtract damage from hitpoints and generate damage animation
        health -= damage;
        AudioManager.instance.PlaySFX(2);
        Instantiate(hitEffect, transform.position, transform.rotation);

        // Destroy enemy if out of hitpoints and generate death splatter
        if (health <= 0)
        {
            Destroy(gameObject);
            AudioManager.instance.PlaySFX(1);

            // Generate random splatter
            int selectedSplatter = Random.Range(0, deathSplatters.Length);
            int rotation = Random.Range(0, 360);
            Instantiate(deathSplatters[selectedSplatter], transform.position, Quaternion.Euler(0f, 0f, rotation));

            // Generate item drop according to drop chance
            if (shouldDropItem)
            {
                float dropChance = Random.Range(0f, 100f);

                if (dropChance < itemDropPercent)
                {
                    int randomItem = Random.Range(0, itemsToDrop.Length);
                    Instantiate(itemsToDrop[randomItem], transform.position, transform.rotation);
                }
            }
        }
    }



    // Method Enemy chase, roam and patrol
    private void EnemyMove()
    {
        if (shouldChasePlayer && Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeToChasePlayer)
        {
            moveDirection = PlayerController.instance.transform.position - transform.position;
        }
        else if (shouldRunAway && Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeToRunAway)
        {
            moveDirection = transform.position - PlayerController.instance.transform.position;
        }
        else if (shouldWander)
        {
            EnemyWander();
        }
        else if (shouldPatrol)
        {
            EnemyPatrol();
        }
    }



    // Method enemy roam
    private void EnemyWander()
    {
        // Move between pausing
        if (wanderCounter > 0)
        {
            wanderCounter -= Time.deltaTime;
            moveDirection = wanderDirection;

            if (wanderCounter <= 0)
            {
                pauseCounter = Random.Range(pauseLength * 0.75f, pauseLength * 1.25f);
            }
        }

        // Pause between moving
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



    // Method patrol
    private void EnemyPatrol()
    {
        moveDirection = patrolPoints[currentPatrolPoint].position - transform.position;

        // When at current waypoint, go to next waypoint
        if (Vector3.Distance(transform.position, patrolPoints[currentPatrolPoint].position) < 0.2f)
        {
            currentPatrolPoint++;

            // At the end of the waypoint list, go to start of the waypoint list
            if (currentPatrolPoint >= patrolPoints.Length)
            {
                currentPatrolPoint = 0;
            }
        }
    }



    // Method shoot
    private void EnemyShoot()
    {
        // Shoot player if player in range
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



    // Method enemy animation
    private void AnimateEnemy()
    {
        // Animation-switch for moving and standing still
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
