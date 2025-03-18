using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public TutorialScript script;

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.root.CompareTag("Player") && !script.hasShown)
        {
            script.PopupTutorial();
        }
    }
}
