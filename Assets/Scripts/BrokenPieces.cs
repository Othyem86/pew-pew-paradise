using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPieces : MonoBehaviour
{
    // Variabeln für die Bewegung der Bruchteile
    public float moveSpeed = 3f;                // REF Bewegungsgeschwindigkeit
    private Vector3 moveDirection;              // Richtung der Bewegung
    public float deceleration = 5f;             // REF Entschleunigungsfaktor
    
    // Variabeln Entfernen der Bruchteile
    public float lifeTime = 3f;                 // REF Lebensdauer Bruchteile
    public SpriteRenderer theSR;                // REF Bruchteil Sprite
    public float fadeSpeed = 2.5f;              // REF Geschwindigkeit bis zur Ausblendung


    // Start is called before the first frame update
    void Start()
    {
        // Bewegungsgeschwindigkeit und -richtung beliebig generieren
        moveDirection.x = Random.Range(-moveSpeed, moveSpeed);
        moveDirection.y = Random.Range(-moveSpeed, moveSpeed);
    }


    // Update is called once per frame
    void Update()
    {
        // Position jedes Frame aktualisieren
        transform.position += moveDirection * Time.deltaTime;

        // Lineare Interpolation zur Entschleunigung
        moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, deceleration * Time.deltaTime);


        // Bruchteile nach Zeitablauf entfernen
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            // Bruchteile langsam ausblenden
            theSR.color = new Color
            (
                theSR.color.r,
                theSR.color.g,
                theSR.color.b,
                Mathf.MoveTowards(theSR.color.a, 0f, fadeSpeed * Time.deltaTime)
            );


            // Wenn ausgeblendet, Bruchteilobjekte entfernen
            if (theSR.color.a == 0f)
            {
                Destroy(gameObject);
            }         
        }
    }
}
