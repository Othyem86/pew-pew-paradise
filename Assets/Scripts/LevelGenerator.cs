using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    // Variabeln Raumgenerierung
    public GameObject layoutRoom;                                           // REF Raumobjekt
    public int distanceToEnd;                                               // REF Anzahl der Räume bis Ausgangraum
    public Transform generatorPoint;                                        // REF Referenzpunkt für die Raumgenerierung
    public enum Direction { up, right, down, left };                        //
    public Direction selectedDirection;                                     //
    public float xOffset = 18f;                                             //
    public float yOffset = 10f;                                             //
    public LayerMask whatIsRoom;                                            //

    // Variabeln Raumverfolgung
    public Color startColor;                                                // REF Startfarbe
    public Color endColor;                                                  // REf Endfarbe
    private GameObject endRoom;                                             //
    private List<GameObject> layoutRoomObjects = new List<GameObject>();    //


    // Start is called before the first frame update
    void Start()
    {
        //
        Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation)
            .GetComponent<SpriteRenderer>()
            .color = startColor;

        //
        selectedDirection = (Direction)Random.Range(0, 4);
        MoveGenerationPoint();


        // 
        for (int i = 0; i < distanceToEnd; i++)
        {
            GameObject newRoom = Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation);
            layoutRoomObjects.Add(newRoom);

            //
            if (i + 1 == distanceToEnd)
            {
                newRoom.GetComponent<SpriteRenderer>().color = endColor;
                layoutRoomObjects.RemoveAt(layoutRoom.count - 1);
                endRoom = newRoom;
            }

            selectedDirection = (Direction)Random.Range(0, 4);
            MoveGenerationPoint();

            //
            while (Physics2D.OverlapCircle(generatorPoint.position, 0.2f, whatIsRoom))
            {
                MoveGenerationPoint();
            }


        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    
    // Switchfunktion Verschiebung des Generationspunkts
    public void MoveGenerationPoint()
    {
        switch (selectedDirection)
        {
            case Direction.up:
                generatorPoint.position += new Vector3(0f, yOffset, 0f);
                break;

            case Direction.down:
                generatorPoint.position += new Vector3(0f, -yOffset, 0f);
                break;

            case Direction.right:
                generatorPoint.position += new Vector3(xOffset, 0f, 0f);
                break;

            case Direction.left:
                generatorPoint.position += new Vector3(-xOffset, 0f, 0f);
                break;
        }
    }
}
