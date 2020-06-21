using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    // Variables level paramenters
    [Header("Level Parameters")]
    public int distanceToEnd;                                               // REF number of rooms until exit room
    public bool includeGunRoom;                                             // REF if level should have a gun room
    public int minDistanceToGunRoom;                                        // REF maximal number of rooms until gun room
    public int maxDistanceToGunRoom;                                        // REF minimal number of rooms until gun room
    public bool includeShop;                                                // REF if level should have a shop room
    public int minDistanceToShop;                                           // REF maximal number of rooms until shop room
    public int maxDistanceToShop;                                           // REF minimal number of rooms until shop room

    // Variables room generation
    [Header("Room Generation")]
    public GameObject layoutRoom;                                           // REF room stamp for first layout generation
    public GameObject parentRoom;                                           // REF parent room for room walls, room centers and doors
    public Transform generatorPoint;                                        // REF generator point for first layout generation
    public enum Direction { up, right, down, left };                        // REF possible movement directions of generator point
    public Direction selectedDirection;                                     // REF chosed movement direction of generator point
    public float xOffset = 18f;                                             // REF movement length of generator point on the x-axis
    public float yOffset = 10f;                                             // REF movement length of generator point on the y-axis
    public LayerMask whatIsRoom;                                            // REF Layer of generator point collision check

    // Variables room tracking
    [Header("Room Colors")]
    public Color startColor;                                                // REF color start room stamp
    public Color endColor;                                                  // REF color exit room stamp
    public Color shopColor;                                                 // REF color shop room stamp
    public Color gunRoomColor;                                              // REF color gun room stamp
    private GameObject endRoom;                                             // REF room center exit
    private GameObject shopRoom;                                            // REF room center shop
    private GameObject gunRoom;                                             // REF room center gun chest
    private List<GameObject> layoutRoomObjects = new List<GameObject>();    // REF list of all generated room layout stamps
    private List<GameObject> generatedOutlines = new List<GameObject>();    // REF list of all generated parent rooms
    public RoomWallPrefabs roomWalls;                                       // REF container class with all potential wall and door game objects

    // Variables room centers
    [Header("Room Centers")]
    public RoomCenter centerStart;                                          // REF room center start
    public RoomCenter centerGunRoom;                                        // REF room center gun chest
    public RoomCenter centerShop;                                           // REF room center shop
    public RoomCenter centerEnd;                                            // REF room center exit
    public RoomCenter[] potentialCenters;                                   // REF array with all potential room center game objects


    // Start is called before the first frame update
    void Start()
    {
        GenerateLevelLayout();
        CreateRoomOutlineLayout();
        CreateRoomCenters();
    }


    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        // DEV TOOL: GENERATE NEW ROOM CONFIGURATION
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
#endif
    }



    //
    //  METHODS
    //

    // Generate room stamp layout
    private void GenerateLevelLayout()
    {
        // Generate start room stamp
        Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation)
            .GetComponent<SpriteRenderer>()
            .color = startColor;

        // Move generator point in random direction
        selectedDirection = (Direction)Random.Range(0, 4);
        MoveGeneratorPoint();


        // Generate rest of the room stamps
        for (int i = 0; i < distanceToEnd; i++)
        {
            GameObject newRoom = Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation);

            // Add room to room layout list
            layoutRoomObjects.Add(newRoom);

            // Remove exit room from layout list, save it separatly
            if (i + 1 == distanceToEnd)
            {
                newRoom.GetComponent<SpriteRenderer>().color = endColor;
                layoutRoomObjects.RemoveAt(layoutRoomObjects.Count - 1);
                endRoom = newRoom;
            }


            // Move generator point in random direction
            selectedDirection = (Direction)Random.Range(0, 4);
            MoveGeneratorPoint();


            // If a stamp exists at location, move generator point again in random direction
            while (Physics2D.OverlapCircle(generatorPoint.position, 0.2f, whatIsRoom))
            {
                MoveGeneratorPoint();
            }
        }

        // Generate shop room stamp, remove it from layout list and save it separatly
        if (includeShop)
        {
            int shopSelector = Random.Range(minDistanceToShop, maxDistanceToShop);
            shopRoom = layoutRoomObjects[shopSelector];
            layoutRoomObjects.RemoveAt(shopSelector);
            shopRoom.GetComponent<SpriteRenderer>().color = shopColor;
        }

        // Generate gun room stamp, remove it from layout list and save it separatly
        if (includeGunRoom)
        {
            int gunRoomSelector = Random.Range(minDistanceToGunRoom, maxDistanceToGunRoom);
            gunRoom = layoutRoomObjects[gunRoomSelector];
            layoutRoomObjects.RemoveAt(gunRoomSelector);
            gunRoom.GetComponent<SpriteRenderer>().color = gunRoomColor;
        }
    }



    // Move generator point
    private void MoveGeneratorPoint()
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


    private void CreateRoomOutlineLayout()
    {
        // Generate start parent room and its doors and walls
        CreateRoomOutline(Vector3.zero);

        // Generate rest of the parent room and its walls
        foreach (GameObject room in layoutRoomObjects)
        {
            CreateRoomOutline(room.transform.position);
        }

        // Generate exit parent room and its doors and walls
        CreateRoomOutline(endRoom.transform.position);

        // Generate shop parent room and its doors and walls
        if (includeShop)
        {
            CreateRoomOutline(shopRoom.transform.position);
        }

        // Generate gun parent room and its doors and walls
        if (includeGunRoom)
        {
            CreateRoomOutline(gunRoom.transform.position);
        }
    }



    // Generate parent room and its walls
    private void CreateRoomOutline(Vector3 roomPosition)
    {
        // Check if room stamp has adjacent neighbors
        bool roomAbove = Physics2D.OverlapCircle( roomPosition + new Vector3(0f, yOffset, 0f), 0.2f, whatIsRoom );
        bool roomBelow = Physics2D.OverlapCircle( roomPosition + new Vector3(0f, -yOffset, 0f), 0.2f, whatIsRoom );
        bool roomLeft = Physics2D.OverlapCircle( roomPosition + new Vector3(-xOffset, 0f, 0f), 0.2f, whatIsRoom );
        bool roomRight = Physics2D.OverlapCircle( roomPosition + new Vector3(xOffset, 0f, 0f), 0.2f, whatIsRoom );

        GameObject newParentRoom = Instantiate(parentRoom, roomPosition, transform.rotation);
        generatedOutlines.Add(newParentRoom);


        // Generate room wall top
        if (roomAbove)
        {
            Instantiate(roomWalls.doorUp, roomPosition, transform.rotation, newParentRoom.transform);
        }
        else
        {
            Instantiate(roomWalls.wallUp, roomPosition, transform.rotation, newParentRoom.transform);
        }


        // Generate room wall bottom
        if (roomBelow)
        {
            Instantiate(roomWalls.doorDown, roomPosition, transform.rotation, newParentRoom.transform);
        }
        else
        {
            Instantiate(roomWalls.wallDown, roomPosition, transform.rotation, newParentRoom.transform);
        }


        // Generate room wall left
        if (roomLeft)
        {
            Instantiate(roomWalls.doorLeft, roomPosition, transform.rotation, newParentRoom.transform);
        }
        else
        {
            Instantiate(roomWalls.wallLeft, roomPosition, transform.rotation, newParentRoom.transform);
        }


        // Generate room wall right
        if (roomRight)
        {
            Instantiate(roomWalls.doorRight, roomPosition, transform.rotation, newParentRoom.transform);
        }
        else
        {
            Instantiate(roomWalls.wallRight, roomPosition, transform.rotation, newParentRoom.transform);
        }
    }



    // Generate room centers
    private void CreateRoomCenters()
    {
        foreach (GameObject parentOutline in generatedOutlines)
        {
            if (parentOutline.transform.position == Vector3.zero)
            {
                // Generate start room center
                Instantiate(centerStart, parentOutline.transform.position, transform.rotation).theRoom = parentOutline.GetComponent<Room>();
            }
            else if (parentOutline.transform.position == endRoom.transform.position)
            {
                // Generate exit room center
                Instantiate(centerEnd, parentOutline.transform.position, transform.rotation).theRoom = parentOutline.GetComponent<Room>();
            }
            else if (parentOutline.transform.position == shopRoom.transform.position)
            {
                // Generate shop room center
                Instantiate(centerShop, parentOutline.transform.position, transform.rotation).theRoom = parentOutline.GetComponent<Room>();
            }
            else if (parentOutline.transform.position == gunRoom.transform.position)
            {
                // Generate gun room center
                Instantiate(centerGunRoom, parentOutline.transform.position, transform.rotation).theRoom = parentOutline.GetComponent<Room>();
            }
            else
            {
                // Generate random standard room center
                int randomRoomCenter = Random.Range(0, potentialCenters.Length);
                Instantiate(potentialCenters[randomRoomCenter], parentOutline.transform.position, transform.rotation).theRoom = parentOutline.GetComponent<Room>();
            }
        }
    }
}



// Container class with all potential wall and door game objects
[System.Serializable]
public class RoomWallPrefabs
{
    public GameObject doorUp, doorDown, doorLeft, doorRight,
        wallUp, wallDown, wallLeft, wallRight;
}