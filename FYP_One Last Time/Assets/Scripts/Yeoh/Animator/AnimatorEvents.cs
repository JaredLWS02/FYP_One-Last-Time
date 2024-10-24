using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEvents : MonoBehaviour
{
    public GameObject owner;
    public Animator anim;

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.PlayAnimEvent += OnPlayAnim;
    }
    void OnDisable()
    {
        EventM.PlayAnimEvent -= OnPlayAnim;
    }
    
    // ============================================================================
    
    void OnPlayAnim(GameObject who, string animName, int animLayer, float blendSeconds)
    {
        if(who!=owner) return;

        if(blendSeconds <= 0)
        {
            anim.Play(animName, animLayer);
        }
        else
        {
            anim.CrossFade(animName, blendSeconds, animLayer);
        }
    }
}
