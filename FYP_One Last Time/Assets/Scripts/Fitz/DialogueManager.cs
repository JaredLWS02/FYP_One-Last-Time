using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{

    public TMP_Text nameText;
    public TMP_Text dialogueText;

    public Animator animator;

    //private Queue<string> sentences;
    private Queue<Dialogue.DialogueLine> sentences;

    bool isFirstSentence = false;

    // Use this for initialization
    void Start()
    {
        //sentences = new Queue<string>();
        sentences = new Queue<Dialogue.DialogueLine>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("IsOpen", true);

        //nameText.text = dialogue.name;

        sentences.Clear();

        //foreach (string sentence in dialogue.sentences)
        foreach (Dialogue.DialogueLine line in dialogue.lines)
        {
            sentences.Enqueue(line);
        }

        isFirstSentence = true;
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        dialogueText.text = "";

        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        //string sentence = sentences.Dequeue();
        Dialogue.DialogueLine line = sentences.Dequeue();
        nameText.text = line.name;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(line.sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        if (isFirstSentence)
        {
            yield return new WaitForSeconds(0.5f);
            isFirstSentence = false;
        }

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }

        yield return new WaitForSeconds(5.0f);
        DisplayNextSentence();
    }

    void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
    }

}
