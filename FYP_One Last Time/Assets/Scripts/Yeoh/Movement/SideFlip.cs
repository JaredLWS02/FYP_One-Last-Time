using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideFlip : TurnScript
{
    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.FlipEvent += OnFlip;
        EventM.CancelFlipDelayEvent += OnCancelFlipDelay;
    }
    void OnDisable()
    {
        EventM.FlipEvent -= OnFlip;
        EventM.CancelFlipDelayEvent -= OnCancelFlipDelay;

    }

    // ============================================================================

    [Header("Side Flip")]
    public bool faceR=true;
    public bool reverse;

    public void OnFlip(GameObject who, float dir_x)
    {
        if(who!=owner) return;

        if(dir_x==0) return;

        if(isFlipDelaying) return;

        if(reverse)
        {
            if((dir_x>0 && faceR) || (dir_x<0 && !faceR))
            {
                StartFlipDelay();
            }
        }
        else
        {
            if((dir_x<0 && faceR) || (dir_x>0 && !faceR))
            {
                StartFlipDelay();
            }
        }
    }

    // ============================================================================

    [Header("Delay")]
    public Vector2 flipDelay = new(0,.2f);

    void StartFlipDelay()
    {
        if(FlipDelaying_crt!=null) StopCoroutine(FlipDelaying_crt);
        FlipDelaying_crt = StartCoroutine(FlipDelaying());
    }

    bool isFlipDelaying;

    Coroutine FlipDelaying_crt;

    IEnumerator FlipDelaying()
    {
        isFlipDelaying=true;

        float t = Random.Range(flipDelay.x, flipDelay.y);
        yield return new WaitForSeconds(t);
        Flip();

        isFlipDelaying=false;
    }

    // ============================================================================

    void OnCancelFlipDelay(GameObject who)
    {
        if(who!=owner) return;

        if(!isFlipDelaying) return;

        if(FlipDelaying_crt!=null) StopCoroutine(FlipDelaying_crt);

        Flip();

        isFlipDelaying=false;
    }

    // ============================================================================

    void Flip()
    {
        faceR=!faceR;
        //transform.Rotate(0, 180, 0);

        if(sprite)
        sprite.flipX = !faceR;
    }

    [Header("If Using Billboard")]
    public SpriteRenderer sprite;
    
    // ============================================================================

    void FixedUpdate()
    {
        UpdateTurn(faceR ? Vector3.right : Vector3.left);
    }
}
