using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
    public Animator animator;
    public bool isShowing = false, hasShown = false;
    float dTime = 0f;

    void Start()
    {

    }

    void Update()
    {
        if (isShowing)
        {
            dTime += Time.deltaTime;
            if (dTime > 6f)
            {
                animator.SetBool("isShown", false);
            }
        }
    }

    public void PopupTutorial()
    {
        animator.SetBool("isShown", true);
        hasShown = true;
        isShowing = true;
    }
}
