using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorPlayer : MonoBehaviour
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
    
    void OnPlayAnim(GameObject who, string animName, int animLayer, float blendTime)
    {
        if(who!=owner) return;

        if(blendTime <= 0)
        {
            anim.Play(animName, animLayer, 0);
        }
        else
        {
            anim.CrossFade(animName, blendTime, animLayer, 0);
        }
    }
}
