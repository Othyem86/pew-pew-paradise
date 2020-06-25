using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    // Instancing the class
    public static BossController instance;

    // Variables movement and shooting
    [Header("Shooting")]
    private float shotCounter;              // Countdown until the next bullet
    private Vector2 moveDirection;          // REF direction vector of boss
    public Rigidbody2D theRB;               // REF rigid body of boss

    // Variables boss miscellaneous
    [Header("Miscellaneous")]
    public int currentHealth;               // REF Boss current Health
    public GameObject deathEffect;          // REF death effect of boss
    public GameObject hitEffect;            // REF hit effect of boss
    public GameObject levelExit;            // REF level exit

    // Variables Sequence manager
    [Header("Boss Fight Sequences")]
    public BossSequence[] sequences;        // REF boss sequences
    public int currentSequence;             // REF current boss sequence
    private BossAction[] actions;           // REF boss action array
    private int currentAction;              // REF current boss action
    private float actionDuration;           // REF duration until next action



    // Before Start()
    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        actions = sequences[currentSequence].actions;
        actionDuration = actions[currentAction].actionLength;

        UIController.instance.bossHealthBar.maxValue = currentHealth;
        UIController.instance.bossHealthBar.value = currentHealth;
    }


    // Update is called once per frame
    void Update()
    {
        if (actionDuration > 0)
        {
            actionDuration -= Time.deltaTime;

            // TO DO: ShootAtPlayer();
            BossMove();
            BossShotVolley();
        }
        else
        {
            NextAction();
        }
    }



    //
    //  METHODS
    //

    // Boss volley
    private void BossShotVolley()
    {
        if (actions[currentAction].shouldShoot)
        {
            shotCounter -= Time.deltaTime;

            if (shotCounter <= 0)
            {
                shotCounter = actions[currentAction].timeBetweenShots;

                // Shoot form each boss hardpoint
                foreach (Transform point in actions[currentAction].shootingPoints)
                {
                    Instantiate(actions[currentAction].itemToShoot, point.position, point.rotation);
                }
            }
        }
    }



    // Boss movement
    private void BossMove()
    {
        moveDirection = Vector2.zero;

        if (actions[currentAction].shouldMove)
        {
            // Chase player
            if (actions[currentAction].shouldChasePlayer)
            {
                moveDirection = PlayerController.instance.transform.position - transform.position;
                moveDirection.Normalize();
            }

            // Move next waypoint if boss should move to next waypoint, or in the near vecinity of current one
            if (actions[currentAction].shouldMoveToWaypoint && Vector3.Distance(transform.position, actions[currentAction].waypoint.position) > 0.5f)
            {
                moveDirection = actions[currentAction].waypoint.position - transform.position;
                moveDirection.Normalize();
            }
        }

        // Boss movement speed
        theRB.velocity = moveDirection * actions[currentAction].moveSpeed;
    }



    // Damage boss and move to next boss sequence if necessary
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        AudioManager.instance.PlaySFX(2);

        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);

            Instantiate(deathEffect, transform.position, transform.rotation);

            // Spawn exit to the side of the player, if player sits on its spawn position
            if (Vector3.Distance(PlayerController.instance.transform.position, levelExit.transform.position) < 2f )
            {
                levelExit.transform.position += new Vector3(4f, 0f, 0f);
            }

            levelExit.SetActive(true);

            UIController.instance.bossHealthBar.gameObject.SetActive(false);
        }
        else
        {
            if (currentHealth <= sequences[currentSequence].endSequenceHealth && currentSequence < sequences.Length - 1)
            {
                currentSequence++;
                actions = sequences[currentSequence].actions;
                currentAction = 0;
                actionDuration = actions[currentAction].actionLength;
            }
        }

        UIController.instance.bossHealthBar.value = currentHealth;
    }


    // Change to next action
    private void NextAction()
    {
        currentAction++;

        if (currentAction >= actions.Length)
        {
            currentAction = 0;
        }

        actionDuration = actions[currentAction].actionLength;
    }
}



// Boss actions class
[System.Serializable]
public class BossAction
{
    [Header("Action")]
    public float actionLength;              // REF boss action duration

    [Header("Movement")]
    public bool shouldMove;                 // REF if boss should move
    public bool shouldChasePlayer;          // REF if boss should chase player
    public bool shouldMoveToWaypoint;       // REF if boss should move to waypoint
    public float moveSpeed;                 // REB boss movement speed
    public Transform waypoint;              // REF waypoint boss should move to

    [Header("Shoot Volley")]
    public bool shouldShoot;                // REF if boss should shoot
    public GameObject itemToShoot;          // REF bullet boss should shoot
    public float timeBetweenShots;          // REF duration until next volley
    public Transform[] shootingPoints;      // REF array of boss bullet origins

}



// Boss sequences class
[System.Serializable]
public class BossSequence
{
    [Header("Sequence")]
    public BossAction[] actions;            // REF array of boss actions
    public int endSequenceHealth;           // REF boss hitpoints value that triggers next sequence
}
