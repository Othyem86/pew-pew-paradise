using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Instanzierung der Klasse
    public static LevelManager instance;

    // Variabeln Szenenübergang
    public float waitToLoad = 4f;       // REF Dauer bis nächste Szene
    public string nextLevel;            // REF nächste Szene


    // Wie Start(), nur davor
    public void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    // Co-routine Funktion zur wechseln der Szene
    public IEnumerator LevelEnd()
    {
        // Spielerbewegung ausschalten und Zeit abwarten vor Szenenwechsel
        AudioManager.instance.PlayLevelWin();
        PlayerController.instance.canMove = false;
        UIController.instance.StartFadeToBlack();
        yield return new WaitForSeconds(waitToLoad);
        SceneManager.LoadScene(nextLevel);
    }
}