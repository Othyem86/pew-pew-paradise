using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSortOrder : MonoBehaviour
{
    // Variables layer sorting
    private SpriteRenderer theSR;       // Sprite renderer element


    // Start is called before the first frame update
    void Start()
    {
        // Set variable as the target object's sprite renderer component
        theSR = GetComponent<SpriteRenderer>();

        // Automatic layer-sorting according to the object's y-axis postion
        theSR.sortingOrder = Mathf.RoundToInt(transform.position.y * -10);
    }
}