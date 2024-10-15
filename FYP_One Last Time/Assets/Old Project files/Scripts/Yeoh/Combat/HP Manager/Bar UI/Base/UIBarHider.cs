using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBarHider : MonoBehaviour
{
    public float currentValue;
    public float maxValue=100;

    public bool hideWhenFull=true;
    public bool hideWhenEmpty;

    // ============================================================================

    void OnEnable()
    {
        Reset();
    }

    void Update()
    {
        CheckUIVisibility();
    }

    void CheckUIVisibility()
    {
        if(hideWhenFull && currentValue>=maxValue)
        {
            HideUI();
        }

        if(hideWhenEmpty && currentValue<=0)
        {
            HideUI();
        }

        if(currentValue>0 && currentValue<maxValue)
        {
            ShowUI();
        }
    }

    // Tween Show/Hide ============================================================================

    public TweenAnim targetUI;
    public float animTime=.5f;

    bool canShow=true;
    bool canHide;

    void Reset()
    {
        canShow=true;
        canHide=false;
    }

    void HideUI()
    {
        if(canHide)
        {
            canHide=false;
            targetUI.TweenOut(animTime);
            Invoke(nameof(ToggleShow), animTime);  
        }
    }
    void ShowUI()
    {
        if(canShow)
        {   
            canShow=false;
            targetUI.TweenIn(animTime);
            Invoke(nameof(ToggleHide), animTime);
        }
    }

    void ToggleHide()
    {
        canHide=!canHide;
    }
    void ToggleShow()
    {
        canShow=!canShow;
    }
}
