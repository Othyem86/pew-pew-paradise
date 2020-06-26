using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;                   //
    public Text dialogueText;               //
    public Animator animator;               //
    private Queue<string> sentencesQueue;   //


    // Start is called before the first frame update
    void Start()
    {
        sentencesQueue = new Queue<string>();
    }



    // Open the dialogue window
    public void StartDialogue (Dialogue dialogue)
    {
        LevelManager.instance.otherPause = true;
        LevelManager.instance.ispaused = true;

        animator.SetBool("isOpen", true);

        nameText.text = dialogue.name;

        sentencesQueue.Clear();

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

        string sentence = sentencesQueue.Dequeue();
        dialogueText.text = sentence;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }



    // Display dialogue text slowly
    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
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
    }
}
