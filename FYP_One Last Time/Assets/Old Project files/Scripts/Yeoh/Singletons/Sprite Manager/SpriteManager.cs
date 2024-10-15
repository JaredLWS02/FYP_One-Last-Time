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

    // GETTER
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    public List<SpriteRenderer> GetSpriteRenderers(GameObject target)
    {
        if(!target) return null;

        List<SpriteRenderer> renderers = new();

        renderers.AddRange(target.GetComponents<SpriteRenderer>());
        renderers.AddRange(target.GetComponentsInChildren<SpriteRenderer>());

        return renderers;
    }

    // CHANGE COLOR
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    Dictionary<SpriteRenderer, Color> defColors = new();

    public void RecordColor(GameObject target)
    {
        if(!target) return;

        foreach(SpriteRenderer sr in GetSpriteRenderers(target))
        {
            defColors[sr] = sr.color;
        }
    }

    public void OffsetColor(GameObject target, float rOffset, float gOffset, float bOffset)
    {
        if(!target) return;

        RecordColor(target);

        Color colorOffset = new Color(rOffset, gOffset, bOffset);

        foreach(SpriteRenderer sr in GetSpriteRenderers(target))
        {
            sr.color += colorOffset;
        }
    }

    public void RevertColor(GameObject target)
    {
        if(!target) return;

        foreach(SpriteRenderer sr in GetSpriteRenderers(target))
        {
            if(defColors.ContainsKey(sr))
            {
                sr.color = defColors[sr];

                defColors.Remove(sr); // clean up
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

    // RANDOM COLOR
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void RandomOffsetColor(GameObject target, float rOffset=.15f, float gOffset=.15f, float bOffset=.15f, float aOffset=0)
    {
        if(!target) return;

        foreach(SpriteRenderer sr in GetSpriteRenderers(target))
        {
            sr.color = new Color
            (
                sr.color.r + Random.Range(-rOffset, rOffset),
                sr.color.g + Random.Range(-gOffset, gOffset),
                sr.color.b + Random.Range(-bOffset, bOffset),
                sr.color.a + Random.Range(-aOffset, aOffset)
            );
        }
    }

    // RANDOM FLIP
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void RandomFlip(GameObject target, bool flipY=false, bool flipX=true)
    {
        if(!target) return;

        foreach(SpriteRenderer sr in GetSpriteRenderers(target))
        {
            if(flipX) sr.flipX = Random.Range(0, 2)==0 ? true : false;
            if(flipY) sr.flipY = Random.Range(0, 2)==0 ? true : false;
        }
    }

    // COLLIDER BOUNDS
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public List<Collider2D> GetColliders(GameObject target)
    {
        List<Collider2D> colliders = new();

        Collider2D[] colls = target.GetComponents<Collider2D>();
        Collider2D[] childColls = target.GetComponentsInChildren<Collider2D>();

        foreach(Collider2D coll in colls)
        {
            if(!coll.isTrigger) colliders.Add(coll);
        }
        foreach(Collider2D coll in childColls)
        {
            if(!coll.isTrigger) colliders.Add(coll);
        }

        return colliders;
    }

    public Vector3 GetColliderTop(GameObject target)
    {
        List<Collider2D> colliders = GetColliders(target);
        
        if(colliders.Count==0)
        {
            Debug.LogError($"{name}: Couldn't find any Collider on {target.name}");
            return Vector3.zero;
        }

        float highestPoint = float.MinValue;

        foreach(Collider2D coll in colliders)
        {
            Vector3 topPoint = coll.bounds.max;

            if(highestPoint < topPoint.y) highestPoint = topPoint.y;
        }

        return new Vector3(target.transform.position.x, highestPoint, target.transform.position.z);
    }

    public Vector3 GetColliderCenter(GameObject target)
    {
        List<Collider2D> colliders = GetColliders(target);

        Vector3 center = Vector3.zero;

        // Calculate the average position of all colliders' centers
        foreach(Collider2D col in colliders)
        {
            center += col.bounds.center;
        }

        center /= colliders.Count;

        return center;
    }
}
