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
    Vector2 currentFlipDelay;

    protected override void OnBaseAwake()
    {
        currentFlipDelay = flipDelay;
    }

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

        float t = Random.Range(currentFlipDelay.x, currentFlipDelay.y);
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

    public void SetFlipDelay(Vector2 to) => currentFlipDelay = to;
    public void RevertFlipDelay() => currentFlipDelay = flipDelay;

    // ============================================================================

    void Flip()
    {
        faceR=!faceR;
        //transform.Rotate(0, 180, 0);

        foreach(var sprite in spritesToFlip)
        {
            sprite.flipX = !faceR;
        }
        
        foreach(var tr in transformsToInvert)
        {
            tr.localPosition = Mirror(tr.localPosition, invertPosAxis);
        }
    }

    [Header("If Using Billboard")]
    public List<SpriteRenderer> spritesToFlip = new();

    [Header("If Any Children")]
    public List<Transform> transformsToInvert = new();
    public Vector3Int invertPosAxis = new(1,0,0);

    Vector3 Mirror(Vector3 vector, Vector3Int axis)
    {
        return new
        (
            axis.x>0 ? -vector.x : vector.x,
            axis.y>0 ? -vector.y : vector.y,
            axis.z>0 ? -vector.z : vector.z
        );
    }
    
    // ============================================================================

    void FixedUpdate()
    {
        UpdateTurn(faceR ? Vector3.right : Vector3.left);
    }
}
