using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    // Instancing the class
    public static DialogueController instance;

    // Variables dialogue
    [Header("Dialogue Parameters")]
    public Image npcPortait;                            // REF dialogue speaker portrait image
    public Text npcName;                                // REF dialogue speaker name
    public Text dialogueText;                           // REF dialogue text
    public Animator animator;                           // REF animator for dialogue window
    private Queue<string> sentencesQueue;               // REF queue for the dialogue sentences

    [Header("Dialogue Events")]
    private bool eventsAfterDialogue;                    // REF if events should happen after dialogue ends;
    private GameObject[] gameObjectsToDeactivate;        // REF array of gameonbjects to deactivate after dialogue ends
    private GameObject[] gameObjectsToActivate;          // REF array of gameonbjects to activate after dialogue ends


    // Before Start()
    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        sentencesQueue = new Queue<string>();
    }



    // Open the dialogue window
    public void StartDialogue (Dialogue dialogue)
    {
        this.eventsAfterDialogue = dialogue.eventsAfterDialogue;
        this.gameObjectsToDeactivate = dialogue.gameObjectsToDeactivate;
        this.gameObjectsToActivate = dialogue.gameObjectsToActivate;

        LevelManager.instance.otherPause = true;
        LevelManager.instance.ispaused = true;

        animator.SetBool("isOpen", true);

        npcPortait.sprite = dialogue.npcPortrait;
        npcName.text = dialogue.name;

        sentencesQueue.Clear();

        // Load all dialogue lines in the queue
        foreach (string sentence in dialogue.sentences)
        {
            sentencesQueue.Enqueue(sentence);
        }

        DisplayNextSentence();
    }



    // Display the next dialogue sentence
    public void DisplayNextSentence()
    {
        if (sentencesQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        // Dequeue each dialogue line one by one
        string sentence = sentencesQueue.Dequeue();
        dialogueText.text = sentence;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }



    // Coroutine display dialogue text slowly
    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;

            // Frames between each individual letters
            for (int i = 0; i < 10; i++)
            {
                yield return null;
            }
        }
    }



    // Close the dialogue window
    public void EndDialogue()
    {
        animator.SetBool("isOpen", false);

        LevelManager.instance.otherPause = false;
        LevelManager.instance.ispaused = false;

        // Handle Events after dialogue
        if (eventsAfterDialogue)
        {
            foreach (GameObject element in gameObjectsToDeactivate)
            {
                element.SetActive(false);
            }

            foreach (GameObject element in gameObjectsToActivate)
            {
                element.SetActive(true);
            }
        }
    }
}
