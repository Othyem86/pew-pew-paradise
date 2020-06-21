using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    // Variables Levelparameter
    [Header("Level Parameters")]
    public int distanceToEnd;                                               // REF Anzahl der Räume bis Ausgangraum
    public bool includeGunRoom;                                             // REF Ob es einen Waffenraum haben soll
    public int minDistanceToGunRoom;                                        // REF Mindestanzahl Räume bis Waffenraum
    public int maxDistanceToGunRoom;                                        // REF Mindestanzahl Räume bis Waffenraum
    public bool includeShop;                                                // REF Ob es einen Shop haben soll
    public int minDistanceToShop;                                           // REF Mindestanzahl Räume bis Shopraum
    public int maxDistanceToShop;                                           // REF Mindestanzahl Räume bis Shopraum

    // Variables Raumgenerierung
    [Header("Room Generation")]
    public GameObject layoutRoom;                                           // REF Raumobjekt
    public GameObject parentRoom;                                           //
    public Transform generatorPoint;                                        // REF Referenzpunkt für die Raumgenerierung
    public enum Direction { up, right, down, left };                        // REF Mögliche Richtungen Generationspunkt
    public Direction selectedDirection;                                     // REF Ausgewählte Richtung Generationspunkt
    public float xOffset = 18f;                                             // REF Bewegung Generationspunk x-Achse
    public float yOffset = 10f;                                             // REF Bewegung Generationspunk y-Achse
    public LayerMask whatIsRoom;                                            // REF Layer der iteriert werden soll

    // Variables Raumverfolgung
    [Header("Room Colors")]
    public Color startColor;                                                // REF Startraumfarbe
    public Color endColor;                                                  // REF Endraumfarbe
    public Color shopColor;                                                 // REF Shopfarbe
    public Color gunRoomColor;                                              // REF Waffenraumfarbe
    private GameObject endRoom;                                             // REF Raumobjekt Levelende
    private GameObject shopRoom;                                            // REF Raumobjekt Shop
    private GameObject gunRoom;                                             // REF Raumobjekt Waffenraum
    private List<GameObject> layoutRoomObjects = new List<GameObject>();    // REF Liste aller generierten Raum-mockups
    private List<GameObject> generatedOutlines = new List<GameObject>();    // REF Liste aller generierten Raumkontouren
    public RoomWallPrefabs roomWalls;                                       // 

    // Variables Raummitten
    [Header("Room Centers")]
    public RoomCenter centerStart;                                          // REF Raummitte Start
    public RoomCenter centerGunRoom;                                        // REF Raummitte Waffenraum
    public RoomCenter centerShop;                                           // REF Raummitte Shop
    public RoomCenter centerEnd;                                            // REF Raummitte Ende
    public RoomCenter[] potentialCenters;                                   // REF Liste potentieller Raummitten


    // Start is called before the first frame update
    void Start()
    {
        // Generate start room layout
        Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation)
            .GetComponent<SpriteRenderer>()
            .color = startColor;

        // Beliebige Richtung zum generieren auswählen
        selectedDirection = (Direction)Random.Range(0, 4);
        MoveGenerationPoint();



        // Layout - Gewünschte Anzahl der Räume generieren
        for (int i = 0; i < distanceToEnd; i++)
        {
            GameObject newRoom = Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation);

            // Raum zur Liste hinzufügen
            layoutRoomObjects.Add(newRoom);

            // Falls letzter Raum, von liste enfernen
            if (i + 1 == distanceToEnd)
            {
                newRoom.GetComponent<SpriteRenderer>().color = endColor;
                layoutRoomObjects.RemoveAt(layoutRoomObjects.Count - 1);
                endRoom = newRoom;
            }


            // beliebige Richtung auswählen, dorthin gehen, Raum generieren
            selectedDirection = (Direction)Random.Range(0, 4);
            MoveGenerationPoint();


            // Falls es an der Position einen Raum gibt, in ausgewählte Richtung weitergehen
            while (Physics2D.OverlapCircle(generatorPoint.position, 0.2f, whatIsRoom))
            {
                MoveGenerationPoint();
            }
        }



        // Shoplayout generieren
        if (includeShop)
        {
            int shopSelector = Random.Range(minDistanceToShop, maxDistanceToShop);
            shopRoom = layoutRoomObjects[shopSelector];
            layoutRoomObjects.RemoveAt(shopSelector);
            shopRoom.GetComponent<SpriteRenderer>().color = shopColor;
        }
        
        
        
        // Waffenraumlayout generieren
        if (includeGunRoom)
        {
            int gunRoomSelector = Random.Range(minDistanceToGunRoom, maxDistanceToGunRoom);
            gunRoom = layoutRoomObjects[gunRoomSelector];
            layoutRoomObjects.RemoveAt(gunRoomSelector);
            gunRoom.GetComponent<SpriteRenderer>().color = gunRoomColor;
        }



        // Generiere Startraumkontour
        CreateRoomOutline(Vector3.zero);

        // Generiere Zwischenraumkontouren
        foreach (GameObject room in layoutRoomObjects)
        {
            CreateRoomOutline(room.transform.position);
        }

        // Generiere Endraumkonturen
        CreateRoomOutline(endRoom.transform.position);

        // Generiere Shopraumkontour
        if (includeShop)
        {
            CreateRoomOutline(shopRoom.transform.position);
        }

        // Generiere Shopraumkontour
        if (includeGunRoom)
        {
            CreateRoomOutline(gunRoom.transform.position);
        }



        // Raumkontourliste durchgehen und Raummitten erzeugen
        CreateRoomCenters();
    }


    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        // DEV Neue Raumkonfiguration generieren
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
#endif
    }



    //
    //  METHODEN
    //

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


    // Funktion Raumkonturen generieren
    private void CreateRoomOutline(Vector3 roomPosition)
    {
        // Überprüfen ob andere Räume abgrenzen
        bool roomAbove = Physics2D.OverlapCircle( roomPosition + new Vector3(0f, yOffset, 0f), 0.2f, whatIsRoom );
        bool roomBelow = Physics2D.OverlapCircle( roomPosition + new Vector3(0f, -yOffset, 0f), 0.2f, whatIsRoom );
        bool roomLeft = Physics2D.OverlapCircle( roomPosition + new Vector3(-xOffset, 0f, 0f), 0.2f, whatIsRoom );
        bool roomRight = Physics2D.OverlapCircle( roomPosition + new Vector3(xOffset, 0f, 0f), 0.2f, whatIsRoom );

        GameObject newParentRoom = Instantiate(parentRoom, roomPosition, transform.rotation);
        generatedOutlines.Add(newParentRoom);

        //
        if (roomAbove)
        {
            Instantiate(roomWalls.doorUp, roomPosition, transform.rotation, newParentRoom.transform);
        }
        else
        {
            Instantiate(roomWalls.wallUp, roomPosition, transform.rotation, newParentRoom.transform);
        }

        //
        if (roomBelow)
        {
            Instantiate(roomWalls.doorDown, roomPosition, transform.rotation, newParentRoom.transform);
        }
        else
        {
            Instantiate(roomWalls.wallDown, roomPosition, transform.rotation, newParentRoom.transform);
        }


        //
        if (roomLeft)
        {
            Instantiate(roomWalls.doorLeft, roomPosition, transform.rotation, newParentRoom.transform);
        }
        else
        {
            Instantiate(roomWalls.wallLeft, roomPosition, transform.rotation, newParentRoom.transform);
        }

        //
        if (roomRight)
        {
            Instantiate(roomWalls.doorRight, roomPosition, transform.rotation, newParentRoom.transform);
        }
        else
        {
            Instantiate(roomWalls.wallRight, roomPosition, transform.rotation, newParentRoom.transform);
        }
    }



    // Funktion Raummitten generieren
    private void CreateRoomCenters()
    {
        foreach (GameObject outline in generatedOutlines)
        {
            if (outline.transform.position == Vector3.zero)
            {
                // Falls es die Startraumkoordinaten sind, Startraum erzeugen
                Instantiate(centerStart, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
            }
            else if (outline.transform.position == endRoom.transform.position)
            {
                // Falls es die Endraumkoordinaten sind, Endraum erzeugen
                Instantiate(centerEnd, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
            }
            else if (outline.transform.position == shopRoom.transform.position)
            {
                // Falls es die Endraumkoordinaten sind, Shopraum erzeugen
                Instantiate(centerShop, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
            }
            else if (outline.transform.position == gunRoom.transform.position)
            {
                // Falls es die Endraumkoordinaten sind, Shopraum erzeugen
                Instantiate(centerGunRoom, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
            }
            else
            {
                // Sonst beliebige Raummitten der Zwischenräume erstellen
                int randomRoomCenter = Random.Range(0, potentialCenters.Length);
                Instantiate(potentialCenters[randomRoomCenter], outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
            }
        }
    }
}


// REF Klasse aller möglichen Raumeingangstypen
[System.Serializable]
public class RoomWallPrefabs
{
    public GameObject doorUp, doorDown, doorLeft, doorRight,
        wallUp, wallDown, wallLeft, wallRight;
}