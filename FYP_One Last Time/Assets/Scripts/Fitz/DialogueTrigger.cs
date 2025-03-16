using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    public Dialogue dialogue;
    public DialogueManager manager;
    bool hasSpoken = false;

    public void TriggerDialogue()
    {
        //FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        manager.StartDialogue(dialogue);
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.root.CompareTag("Player") && !hasSpoken)
        {
            hasSpoken = true;
            Debug.Log("Player has entered dialogue box trigger");
            TriggerDialogue();
        }
    }

}
