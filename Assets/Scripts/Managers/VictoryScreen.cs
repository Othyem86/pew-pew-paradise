using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour
{
    // Variables victory screen
    public float waitForAnyKey = 2f;    // REF wait duration
    public GameObject anyKeyText;       // REF text 'press any key'
    public string mainMenuScene;        // REF main menu scene


    // Start is called before the first frame update
    void Start()
    {
        // Set game speed to 1
        Time.timeScale = 1;
        Destroy(PlayerController.instance.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        // If countdown not zero, start or resume it
        if (waitForAnyKey > 0)
        {
            waitForAnyKey -= Time.deltaTime;
            if (waitForAnyKey <= 0)
            {
                anyKeyText.SetActive(true);
            }
        }
        else 
        {
            // Return to main menu scene if any key is pressed
            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene(mainMenuScene);
            }
        }
    }
}
