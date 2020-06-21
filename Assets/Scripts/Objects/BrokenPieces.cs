using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPieces : MonoBehaviour
{
    // Variables broken pieces movement
    public float moveSpeed = 3f;                // REF movement speed pieces
    private Vector3 moveDirection;              // Direction vector of the pieces
    public float deceleration = 5f;             // REF deceleration of pieces movement

    // Variables remove broken pieces
    public float lifeTime = 3f;                 // REF lifetime broken pieces
    public SpriteRenderer theSR;                // REF sprite renderer broken pieces
    public float fadeSpeed = 2.5f;              // REF how fast pieces disappear


    // Start is called before the first frame update
    void Start()
    {
        // Generate random speed and directon for the broken pieces
        moveDirection.x = Random.Range(-moveSpeed, moveSpeed);
        moveDirection.y = Random.Range(-moveSpeed, moveSpeed);
    }


    // Update is called once per frame
    void Update()
    {
        MoveBrokenPieces();
        FadeOutBrokenPieces();
    }



    //
    //  METHODS
    //

    // Move broken pieces
    private void MoveBrokenPieces()
    {
        transform.position += moveDirection * Time.deltaTime;

        // Linear interpolation for deceleration
        moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, deceleration * Time.deltaTime);
    }



    // Remove pieces after lifetime comes to an end
    private void FadeOutBrokenPieces()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            // Fade away pieces
            theSR.color = new Color
            (
                theSR.color.r,
                theSR.color.g,
                theSR.color.b,
                Mathf.MoveTowards(theSR.color.a, 0f, fadeSpeed * Time.deltaTime)
            );

            // Destroy pieces once they faded away
            if (theSR.color.a == 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}