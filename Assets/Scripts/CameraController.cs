using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Instanzierung der Klasse
    public static CameraController instance;

    // Variabeln Kamerasteuerung
    public float moveSpeed;         // Geschwindigkeit der Kamera
    public Transform target;        // Zielorientierung der Kamera


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


    // Funktion zur Bewegung der Kamera
    public void ChangeCameraTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
