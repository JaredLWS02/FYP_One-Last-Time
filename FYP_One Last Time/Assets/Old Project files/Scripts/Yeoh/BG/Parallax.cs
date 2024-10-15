using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    SpriteRenderer sr;

    void Awake()
    {
        sr=GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        startPos = transform.position;
    }

    // ============================================================================

    public bool fixedUpdate = true;

    void Update()
    {
        if(fixedUpdate) return;

        Offset();
        Loop();
    }
    
    void FixedUpdate()
    {
        if(!fixedUpdate) return;

        Offset();
        Loop();
    }

    // ============================================================================

    Vector2 startPos;
    Vector2 offsetPos;

    public Vector2 parallax = new Vector2(.9f, .9f);

    public bool enableX=true, enableY=true;

    void Offset()
    {
        offsetPos = Camera.main.transform.position * parallax;

        offsetPos.x = enableX ? offsetPos.x : 0;
        offsetPos.y = enableY ? offsetPos.y : 0;

        transform.position = startPos + offsetPos;
    }

    // ============================================================================
    
    public bool loopX=true, loopY=true;

    void Loop()
    {
        Vector2 adjustedCamPos = GetAdjustedCamPos();
        
        if(loopX)
        {
            UpdateStartPos(ref startPos.x, adjustedCamPos.x, sr.bounds.size.x);
        }
        if(loopY)
        {
            UpdateStartPos(ref startPos.y, adjustedCamPos.y, sr.bounds.size.y);
        }
    }

    void UpdateStartPos(ref float startPos, float adjustedCamPos, float bgSize)
    {
        float halfSize = bgSize * 0.5f;

        if(adjustedCamPos > startPos + halfSize)
        {
            startPos += bgSize;
        }
        else if(adjustedCamPos < startPos - halfSize)
        {
            startPos -= bgSize;
        }
    }

    Vector2 GetAdjustedCamPos()
    {
        return Camera.main.transform.position * (Vector2.one - parallax);
    }
}
