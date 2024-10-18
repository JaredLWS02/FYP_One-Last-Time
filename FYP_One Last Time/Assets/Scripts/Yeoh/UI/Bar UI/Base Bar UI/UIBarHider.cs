using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBarHider : MonoBehaviour
{
    public float value;
    public float maxValue=1;

    public bool hideWhenFull=true;
    public bool hideWhenEmpty;

    // ============================================================================
    
    void Awake()
    {
        Reset();
    }
    
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
        if(hideWhenFull && value>=maxValue)
        {
            HideUI();
        }

        if(hideWhenEmpty && value<=0)
        {
            HideUI();
        }

        if(value>0 && value<maxValue)
        {
            ShowUI();
        }
    }

    // ============================================================================

    public TweenAnim targetUI;
    public float animTime=.5f;

    bool canShow=true;
    bool canHide=false;

    void Reset()
    {
        canShow=true;
        canHide=false;
    }

    void HideUI()
    {
        if(!canHide) return;
        canHide=false;

        if(hidingUI_crt!=null) StopCoroutine(hidingUI_crt);
        hidingUI_crt = StartCoroutine(HidingUI());
    }
    void ShowUI()
    {
        if(!canShow) return;
        canShow=false;

        if(showingUI_crt!=null) StopCoroutine(showingUI_crt);
        showingUI_crt = StartCoroutine(ShowingUI());
    }

    //  ============================================================================

    Coroutine hidingUI_crt;

    IEnumerator HidingUI()
    {
        targetUI.TweenOut(animTime);
        yield return new WaitForSecondsRealtime(animTime);
        canShow=true;
    }

    Coroutine showingUI_crt;

    IEnumerator ShowingUI()
    {
        targetUI.TweenIn(animTime);
        yield return new WaitForSecondsRealtime(animTime);
        canHide=true;
    }
}
