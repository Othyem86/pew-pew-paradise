using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    // Schaden anrichten wenn Spieler im Feld reingeht
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerHealthController.instance.DamagePlayer();
        }
    }


    // Schaden anrichten wenn Spieler im Feld bleibt
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerHealthController.instance.DamagePlayer();
        }
    }


    // Schaden anrichten wenn Spieler mit Feld kollidiert
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerHealthController.instance.DamagePlayer();
        }
    }


    // Schaden anrichten wenn Spieler im Feld bleibt
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerHealthController.instance.DamagePlayer();
        }
    }
}
