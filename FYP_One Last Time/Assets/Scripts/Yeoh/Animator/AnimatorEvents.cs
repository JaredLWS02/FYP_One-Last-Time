using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEvents : MonoBehaviour
{
    public GameObject owner;
    public Animator anim;

    // ============================================================================

    void OnEnable()
    {
        EventManager.Current.PlayAnimEvent += OnPlayAnim;
    }
    void OnDisable()
    {
        EventManager.Current.PlayAnimEvent -= OnPlayAnim;
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
