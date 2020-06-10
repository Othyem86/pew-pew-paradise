using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Instanzierung der Klasse
    public static CameraController instance;

    // Variabeln Kamerasteuerung
    [Header("Camera Movement")]
    public float moveSpeed;         // REF Geschwindigkeit der Kamera
    public Transform target;        // REF Zielorientierung der Kamera

    // Variabeln Karte
    [Header("Map Camera")]
    public Camera mainCamera;       // REF Hauptkamera
    public Camera bigMapCamera;     // REF Kartenkamera
    private bool bigMapActive;      // Ob Kartenkamera aktiv ist
    private bool miniMapActive;     // Ob Minimap aktiv sein soll


    // Wie Start(), nur davor
    public void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        MoveCameraToTarget();
        ToggleBigMap();


        // Minimap per Tastendruck aktivieren
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            miniMapActive = !miniMapActive;
            UIController.instance.mapDisplay.SetActive(miniMapActive);
        }
    }



    //
    //  METHODEN
    //

    // Methode Kameraziel ändern
    public void ChangeCameraTarget(Transform newTarget)
    {
        target = newTarget;
    }



    // Methode Kamera zum Ziel bewegen
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



    // Methode Große Karte per Tastendruck schalten
    private void ToggleBigMap()
    {
        // Große Karte per Tastendruck aktivieren
        if (Input.GetKeyDown(KeyCode.M))
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


    // Methode Minimap per Tastendruck schalten
    private void ToggleMiniMap()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            miniMapActive = !miniMapActive;
            UIController.instance.mapDisplay.SetActive(miniMapActive);
        }
    }



    // Methode große Karte aktivieren
    public void ActivateBigMap()
    {
        if (!LevelManager.instance.ispaused)
        {
            bigMapActive = true;

            bigMapCamera.enabled = true;
            mainCamera.enabled = false;

            // Spiel pausieren und Minimap deaktivieren
            PlayerController.instance.canMove = false;
            Time.timeScale = 0f;

            // Minimap immer deaktivieren
            UIController.instance.mapDisplay.SetActive(false);
        }
    }



    // Methode große Karte deaktivieren
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
        }
    }
}