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
    public enum Direction { up, right, down, left };                        // REF Mögliche Richtungen Generationspunkt
    public Direction selectedDirection;                                     // REF Ausgewählte Richtung Generationspunkt
    public float xOffset = 18f;                                             // REF Bewegung Generationspunk x-Achse
    public float yOffset = 10f;                                             // REF Bewegung Generationspunk y-Achse
    public LayerMask whatIsRoom;                                            // REF Layer der iteriert werden soll

    // Variabeln Raumverfolgung
    public Color startColor;                                                // REF Startraumfarbe
    public Color endColor;                                                  // REF Endraumfarbe
    private GameObject endRoom;                                             // REF Raumobjekt Levelende
    private List<GameObject> layoutRoomObjects = new List<GameObject>();    // REF Liste aller generierten Raum-mockups
    private List<GameObject> generatedOutlines = new List<GameObject>();    // REF Liste aller generierten Raumkontouren
    public RoomPrefabs rooms;                                               // REF alle Raumkonturtypen

    // Variabeln Raummitten
    public RoomCenter centerStart;                                          // REF Raummitte Start
    public RoomCenter centerEnd;                                            // REF Raummitte Ende
    public RoomCenter[] potentialCenters;                                   // REF Liste potentieller Raummitten


    // Start is called before the first frame update
    void Start()
    {
        // Startraum Generieren
        Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation)
            .GetComponent<SpriteRenderer>()
            .color = startColor;

        // Beliebige Richtung zum generieren auswählen
        selectedDirection = (Direction)Random.Range(0, 4);
        MoveGenerationPoint();


        // Gewünschte Anzahl der Räume generieren
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


        // Generiere Startraumkontour
        CreateRoomOutline(Vector3.zero);


        // Generiere Zwischenraumkontouren
        foreach (GameObject room in layoutRoomObjects)
        {
            CreateRoomOutline(room.transform.position);
        }


        // Generiere Endraumkonturen
        CreateRoomOutline(endRoom.transform.position);


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
    //  FUNKTIONEN
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


        // Anzahl der Eingänge
        int directionCount = 0;


        // Eingänge zählen
        if (roomAbove)  { directionCount++; }
        if (roomBelow)  { directionCount++; }
        if (roomLeft)   { directionCount++; }
        if (roomRight)  { directionCount++; }
        

        // Generiere Raumkontouren und addiere sie zu einer Raumkontourliste
        switch (directionCount)
        {
            case 1:
                if (roomAbove) { generatedOutlines.Add( Instantiate(rooms.singleUp, roomPosition, transform.rotation) ); }
                if (roomBelow) { generatedOutlines.Add( Instantiate(rooms.singleDown, roomPosition, transform.rotation) ); }
                if (roomLeft) { generatedOutlines.Add( Instantiate(rooms.singleLeft, roomPosition, transform.rotation) ); }
                if (roomRight) { generatedOutlines.Add( Instantiate(rooms.singleRight, roomPosition, transform.rotation) ); }
                break;

            case 2:
                if (roomAbove && roomBelow) { generatedOutlines.Add(Instantiate(rooms.doubleUpDown, roomPosition, transform.rotation)); }
                if (roomLeft && roomRight) { generatedOutlines.Add(Instantiate(rooms.doubleLeftRight, roomPosition, transform.rotation)); }
                if (roomAbove && roomRight) { generatedOutlines.Add(Instantiate(rooms.doubleUpRight, roomPosition, transform.rotation)); }
                if (roomBelow && roomRight) { generatedOutlines.Add(Instantiate(rooms.doubleDownRight, roomPosition, transform.rotation)); }
                if (roomBelow && roomLeft) { generatedOutlines.Add(Instantiate(rooms.doubleDownLeft, roomPosition, transform.rotation)); }
                if (roomAbove && roomLeft) { generatedOutlines.Add(Instantiate(rooms.doubleLeftUp, roomPosition, transform.rotation)); }
                break;

            case 3:
                if (!roomLeft) { generatedOutlines.Add(Instantiate(rooms.tripleUpRightDown, roomPosition, transform.rotation)); }
                if (!roomAbove) { generatedOutlines.Add(Instantiate(rooms.tripleRightDownLeft, roomPosition, transform.rotation)); }
                if (!roomRight) { generatedOutlines.Add(Instantiate(rooms.tripleDownLeftUp, roomPosition, transform.rotation)); }
                if (!roomBelow) { generatedOutlines.Add(Instantiate(rooms.tripleLeftUpRight, roomPosition, transform.rotation)); }
                break;

            case 4:
                generatedOutlines.Add(Instantiate(rooms.fourWay, roomPosition, transform.rotation));
                break;
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
public class RoomPrefabs
{
    public GameObject singleRight, singleUp, singleLeft, singleDown,
        doubleLeftRight, doubleUpDown, doubleUpRight, doubleDownRight,
        doubleDownLeft, doubleLeftUp, tripleUpRightDown, tripleRightDownLeft,
        tripleDownLeftUp, tripleLeftUpRight, fourWay;
}