using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpriteFlip : MonoBehaviour
{
    public SpriteRenderer sr;
    public bool flipX=true, flipY;

    void Awake()
    {
        if(flipX && Random.Range(0,2)==0)
            sr.flipX = !sr.flipX;

        if(flipY && Random.Range(0,2)==0)
            sr.flipY = !sr.flipY;
    }
}
