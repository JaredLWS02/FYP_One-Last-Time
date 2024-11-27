using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

public class FadeAnim : MonoBehaviour
{
    SpriteRenderer sr;
    Image img;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        img = GetComponent<Image>();
    }

    // ============================================================================
    
    public bool ignoreTimescale;

    Tween alphaTween;

    public void TweenAlpha(float to, float time)
    {
        alphaTween.Stop();

        if(sr)
        {
            if(time>0) alphaTween = Tween.Alpha(sr, to, time, Ease.InOutSine, useUnscaledTime: ignoreTimescale);
            else
            {
                Color color = sr.color;
                color.a = to;
                sr.color = color;
            }
        }
        if(img)
        {
            if(time>0) alphaTween = Tween.Alpha(img, to, time, Ease.InOutSine, useUnscaledTime: ignoreTimescale);
            else
            {
                Color color = img.color;
                color.a = to;
                img.color = color;
            }
        }
    }
}
