using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class Breakables : MonoBehaviour
{
    // Variables broken pieces
    [Header("Broken Pieces")]
    public GameObject[] brokenPieces;           // REF array of broken pieces
    public int maxPieces = 5;                   // REF maximum number of pieces

    // Variabeln für Random Drops
    [Header("Drops")]
    public bool shouldDropItem;                 // REF if should generate a drop
    public GameObject[] itemsToDrop;            // REF array of possilbe drops
    public float itemDropPercent;               // REF drop chance



    //
    //  METHODS
    //
    
    // Collision with player or player bullet
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Destroy original object, generate between 1 and 6 broken pieces
        if (other.tag == "PlayerBullet" || other.tag == "Player" && PlayerController.instance.dashCounter > 0)
        {
            // Destroy object
            Destroy(gameObject);
            int piecesToDrop = Random.Range(1, maxPieces);
            AudioManager.instance.PlaySFX(0);


            // Generate broken pieces
            for (int i = 0; i < piecesToDrop; i++)
            {
                int randomPiece = Random.Range(0, brokenPieces.Length);
                Instantiate(brokenPieces[randomPiece], transform.position, transform.rotation);
            }


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
}