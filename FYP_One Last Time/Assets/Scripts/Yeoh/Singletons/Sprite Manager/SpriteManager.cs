using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public static SpriteManager Current;

    void Awake()
    {
        if(!Current) Current=this;
    }

    // Getters ============================================================================
    
    public List<SpriteRenderer> GetSpriteRenderers(GameObject target)
    {
        if(!target) return null;

        List<SpriteRenderer> renderers = new();

        renderers.AddRange(target.GetComponents<SpriteRenderer>());
        renderers.AddRange(target.GetComponentsInChildren<SpriteRenderer>());

        return renderers;
    }

    // Change Colour ============================================================================

    Dictionary<SpriteRenderer, Color> originalColors = new();

    public void RecordColor(GameObject target)
    {
        if(!target) return;

        foreach(var sr in GetSpriteRenderers(target))
        {
            originalColors[sr] = sr.color;
        }
    }

    public void OffsetColor(GameObject target, float rOffset, float gOffset, float bOffset)
    {
        if(!target) return;

        RecordColor(target);

        Color colorOffset = new(rOffset, gOffset, bOffset);

        foreach(var sr in GetSpriteRenderers(target))
        {
            sr.color += colorOffset;
        }
    }

    public void RevertColor(GameObject target)
    {
        if(!target) return;

        foreach(var sr in GetSpriteRenderers(target))
        {
            if(originalColors.ContainsKey(sr))
            {
                sr.color = originalColors[sr];

                originalColors.Remove(sr); // clean up
            }
        }
    }

    public void FlashColor(GameObject target, float rOffset=0, float gOffset=0, float bOffset=0, float time=.1f)
    {
        if(!target) return;

        if(flashingColorRts.ContainsKey(target))
        {
            if(flashingColorRts[target]!=null) StopCoroutine(flashingColorRts[target]);
        }
        flashingColorRts[target] = StartCoroutine(FlashingColor(target, time, rOffset, gOffset, bOffset));
    }

    Dictionary<GameObject, Coroutine> flashingColorRts = new();

    IEnumerator FlashingColor(GameObject target, float t, float r, float g, float b)
    {
        OffsetColor(target, r, g, b);
        yield return new WaitForSeconds(t);
        RevertColor(target);
    }

    // Random Colour ============================================================================

    public void RandomOffsetColor(GameObject target, float rOffset=.15f, float gOffset=.15f, float bOffset=.15f, float aOffset=0)
    {
        if(!target) return;

        foreach(var sr in GetSpriteRenderers(target))
        {
            sr.color = new
            (
                sr.color.r + Random.Range(-rOffset, rOffset),
                sr.color.g + Random.Range(-gOffset, gOffset),
                sr.color.b + Random.Range(-bOffset, bOffset),
                sr.color.a + Random.Range(-aOffset, aOffset)
            );
        }
    }

    // Random FLip ============================================================================

    public void RandomFlip(GameObject target, bool flipY=false, bool flipX=true)
    {
        if(!target) return;

        foreach(var sr in GetSpriteRenderers(target))
        {
            if(flipX) sr.flipX = Random.Range(0, 2)==0 ? true : false;
            if(flipY) sr.flipY = Random.Range(0, 2)==0 ? true : false;
        }
    }
    
}
