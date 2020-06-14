using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    private bool canSelect;
    public GameObject message;
    public PlayerController playerToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canSelect && Input.GetKeyDown(KeyCode.E))
        {
            Vector3 playerPositon = PlayerController.instance.transform.position;
            Destroy(PlayerController.instance.gameObject);

            PlayerController newPlayer = Instantiate(playerToSpawn, playerPositon, playerToSpawn.transform.rotation);
            PlayerController.instance = newPlayer;

            gameObject.SetActive(false);
            CameraController.instance.target = newPlayer.transform;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canSelect = true;
            message.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canSelect = false;
            message.SetActive(false);
        }
    }
}