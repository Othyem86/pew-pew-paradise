using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Instancing the class
    public static CameraController instance;

    // Variables camera controls
    [Header("Camera Movement")]
    public float moveSpeed;         // REF Geschwindigkeit der Kamera
    public Transform target;        // REF Zielorientierung der Kamera
    public bool isBossRoom;         // If level is a boss room

    // Variables map
    [Header("Map Camera")]
    public Camera mainCamera;       // REF main camera
    public Camera bigMapCamera;     // REF map camera
    private bool bigMapActive;      // If map camera is active
    private bool miniMapActive;     // If minimap is active


    public void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        if (isBossRoom)
        {
            target = PlayerController.instance.transform;
        }
    }


    // Update is called once per frame
    void Update()
    {
        MoveCameraToTarget();
        ToggleBigMap();
        ToggleMiniMap();
    }



    //
    //  METHODS
    //

    // Change camera target
    public void ChangeCameraTarget(Transform newTarget)
    {
        target = newTarget;
    }



    // Move camera to target
    private void MoveCameraToTarget()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards
            (
                transform.position,
                new Vector3(target.position.x, target.position.y, transform.position.z),
                moveSpeed * Time.deltaTime
            );
        }
    }



    // Toggle minimap
    private void ToggleMiniMap()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !bigMapActive && !isBossRoom)
        {
            miniMapActive = !miniMapActive;
            UIController.instance.mapDisplay.SetActive(miniMapActive);
        }
    }



    // Toggle big map camera
    private void ToggleBigMap()
    {
        if (Input.GetKeyDown(KeyCode.M) && !isBossRoom)
        {
            if (bigMapActive)
            {
                DeactivateBigMap();
            }
            else
            {
                ActivateBigMap();
            }
        }
    }



    // Activate big map
    public void ActivateBigMap()
    {
        if (!LevelManager.instance.ispaused)
        {
            bigMapActive = true;

            bigMapCamera.enabled = true;
            mainCamera.enabled = false;

            // Pause game and deactivat minimap
            PlayerController.instance.canMove = false;
            Time.timeScale = 0f;

            // Always deactivate minimap
            UIController.instance.mapDisplay.SetActive(false);

            // Toggle map text
            UIController.instance.miniMapText.SetActive(false);
            UIController.instance.bigMapText.SetActive(true);

        }
    }



    // Deactivate big map
    public void DeactivateBigMap()
    {
        if (!LevelManager.instance.ispaused)
        {
            bigMapActive = false;

            bigMapCamera.enabled = false;
            mainCamera.enabled = true;

            // Continue game
            PlayerController.instance.canMove = true;
            Time.timeScale = 1f;

            // Reactivate minimap only if it was active prior big map
            if (miniMapActive)
            {
                UIController.instance.mapDisplay.SetActive(true);
            }

            // Toggle map texts
            UIController.instance.bigMapText.SetActive(false);
            UIController.instance.miniMapText.SetActive(true);
        }
    }
}