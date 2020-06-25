using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;

    public Animator animator;

    private Queue<string> sentencesQueue;

    // Start is called before the first frame update
    void Start()
    {
        sentencesQueue = new Queue<string>();
    }

    public void StartDialogue (Dialogue dialogue)
    {
        animator.SetBool("isOpen", true);

        nameText.text = dialogue.name;

        sentencesQueue.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentencesQueue.Enqueue(sentence);
        }

        DisplayNextSentence();
    }


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

    void EndDialogue()
    {
        animator.SetBool("isOpen", false);
    }
}
