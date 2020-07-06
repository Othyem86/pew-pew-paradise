using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Instancing the class
    public static PlayerController instance;

    // Variabels movement
    [Header("Movement")]
    public bool canMove = true;                         // REF if player can move
    public float moveSpeed;                             // REF player move speed
    private float activeMoveSpeed;                      // Momentary player move speed
    private Vector2 moveInput;                          // Movement direction input
    public Rigidbody2D theRB;                           // REF player rigid body
    public Transform gunArm;                            // REF coordinates gun arm
    public Animator anim;                               // REF animation
    [HideInInspector] 
    public SpriteRenderer bodySR;                       // REF Body Sprite

    // Variabels dashing
    [Header("Dashing")]
    public float dashSpeed = 8f;                        // REF dash speed
    public float dashLength = 0.5f;                     // REF dash distance
    public float dashCoolDown = 1f;                     // REF dash cooldown
    public float dashInvincDuration = 0.5f;             // REF duration dash invincibility
    private float dashCoolDownCounter;                  // Counter until next dash is available
    [HideInInspector]
    public float dashCounter;                           // Counter until dash invincibility ends

    // Variabels weapons
    [Header("Weapons")]
    public List<Gun> availableGuns = new List<Gun>();   // REF list of all available weapons
    [HideInInspector]
    public int currentGun;                              // Index of the current active weapon

    // Variables Player Mirror
    [HideInInspector]
    public Vector3 mousePos;                            // Mouse screen position
    public Vector3 screenPoint;                         // Player screen position


    // Before Start()
    public void Awake()
    {
        instance = this;

        // Don't destroy player object on scene transition
        DontDestroyOnLoad(gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        // Set standard movement speed
        activeMoveSpeed = moveSpeed;

        UpdateGunUI();
    }


    // Update is called once per frame
    void Update()
    {
        if (canMove && !LevelManager.instance.ispaused)
        {
            MovePlayer();
            PlayerAim();
            PlayerDash();
            AnimatePlayer();
            SelectNextWeapon();
        }
        else
        {
            StopPlayer();
        }
    }



    //
    //  METHODS
    //

    // Move player
    private void MovePlayer()
    {
        // Get user input and normalize the vectors
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();
        theRB.velocity = moveInput * activeMoveSpeed;
    }



    // Stop player
    private void StopPlayer()
    {
        // set player speed to zero
        theRB.velocity = Vector2.zero;
        anim.SetBool("isMoving", false);
    }



    // Player mouse-aim
    private void PlayerAim()
    {
        // Get mouse screen coordinates, trasnform global player position as screen coordinates
        mousePos = Input.mousePosition;
        screenPoint = CameraController.instance.mainCamera.WorldToScreenPoint(transform.localPosition);

        // Mirror player and weapon left/right, towars mouse position
        if (mousePos.x < screenPoint.x)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            gunArm.localScale = new Vector3(-1f, -1f, 1f);
        }
        else
        {
            transform.localScale = Vector3.one;
            gunArm.localScale = Vector3.one;
        }

        // Get weapon vector direction from screen mouse position and screen player position 
        Vector2 offset = new Vector2(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y);

        // Transform weapon vector direction in euler angles
        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

        // Transfrom euler angle to quaternion and set it as the gun arm's rotation
        gunArm.rotation = Quaternion.Euler(0, 0, angle);
    }



    // Player dash
    private void PlayerDash()
    {
        // Dash on spacebar
        if (Input.GetKeyDown(KeyCode.Space) && dashCoolDownCounter <= 0 && dashCounter <= 0)
        {
            activeMoveSpeed = dashSpeed;
            dashCounter = dashLength;
            anim.SetTrigger("dash");

            // Make invincible and play sound
            PlayerHealthController.instance.MakeInvincible(dashInvincDuration);
            AudioManager.instance.PlaySFX(8);
        }

        // Count until dash end
        if (dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;
            if (dashCounter <= 0)
            {
                activeMoveSpeed = moveSpeed;
                dashCoolDownCounter = dashCoolDown;
            }
        }

        // Count until dash is available again
        if (dashCoolDownCounter > 0)
        {
            dashCoolDownCounter -= Time.deltaTime;
        }
    }



    // Method animate player
    private void AnimatePlayer()
    {
        // Switch animation according to player movement
        if (moveInput != Vector2.zero)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
    }



    // Method switch to next weapon
    public void SelectNextWeapon()
    {
        if (Input.GetKeyDown(KeyCode.F) && availableGuns.Count > 0)
        {
            currentGun++;

            if (currentGun >= availableGuns.Count)
            {
                currentGun = 0;
            }

            ActivateGun();
        }
    }



    // Methode activate weapon
    public void ActivateGun()
    {
        // Alle Waffen in der Liste deaktivieren
        foreach (Gun theGun in availableGuns)
        {
            theGun.gameObject.SetActive(false);
        }

        // Actovate weapon
        availableGuns[currentGun].gameObject.SetActive(true);

        UpdateGunUI();
    }



    // Method update the gun in the UI
    public void UpdateGunUI()
    {
        UIController.instance.currentGun.sprite = availableGuns[currentGun].gunUI;
        UIController.instance.currentGunText.text = availableGuns[currentGun].weaponName;
    }
}