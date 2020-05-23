using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSortOrder : MonoBehaviour
{
    private SpriteRenderer theSR;       // Sprite Renderer Element


    // Start is called before the first frame update
    void Start()
    {
        // theSR als den SpriteRendere des jeweiligen Objekts setzen
        theSR = GetComponent<SpriteRenderer>();

        // Automatische Layersortierung der Objekte je nach Position auf de y-Achse
        theSR.sortingOrder = Mathf.RoundToInt(transform.position.y * -10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
