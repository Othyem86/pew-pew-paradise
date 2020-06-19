using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Instantiating the class
    public static CameraController instance;

    // Variabels camera controls
    [Header("Camera Movement")]
    public float moveSpeed;         // REF Geschwindigkeit der Kamera
    public Transform target;        // REF Zielorientierung der Kamera
    public bool isBossRoom;         // if level is a boss room

    // Variabels map
    [Header("Map Camera")]
    public Camera mainCamera;       // REF main camera
    public Camera bigMapCamera;     // REF map camera
    private bool bigMapActive;      // if map camera is active
    private bool miniMapActive;     // if minimap is active




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

    // Method change camera target
    public void ChangeCameraTarget(Transform newTarget)
    {
        target = newTarget;
    }



    // Method move camera to target
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



    // Method toggle minimap
    private void ToggleMiniMap()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !bigMapActive && !isBossRoom)
        {
            miniMapActive = !miniMapActive;
            UIController.instance.mapDisplay.SetActive(miniMapActive);
        }
    }



    // Method toggle big map camera
    private void ToggleBigMap()
    {
        // Große Karte per Tastendruck aktivieren
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



    // Method activate big map
    public void ActivateBigMap()
    {
        if (!LevelManager.instance.ispaused)
        {
            bigMapActive = true;

            bigMapCamera.enabled = true;
            mainCamera.enabled = false;

            // pause game and deactivat minimap
            PlayerController.instance.canMove = false;
            Time.timeScale = 0f;

            // always deactivate minimap
            UIController.instance.mapDisplay.SetActive(false);

            // toggle map text
            UIController.instance.miniMapText.SetActive(false);
            UIController.instance.bigMapText.SetActive(true);

        }
    }



    // Method deactivate big map
    public void DeactivateBigMap()
    {
        if (!LevelManager.instance.ispaused)
        {
            bigMapActive = false;

            bigMapCamera.enabled = false;
            mainCamera.enabled = true;

            // Spiel fortsetzen
            PlayerController.instance.canMove = true;
            Time.timeScale = 1f;

            // Minimap nur reaktivieren, wenn es vorher aktiv war
            if (miniMapActive)
            {
                UIController.instance.mapDisplay.SetActive(true);
            }

            // Kartentexte schalten
            UIController.instance.bigMapText.SetActive(false);
            UIController.instance.miniMapText.SetActive(true);
        }
    }
}