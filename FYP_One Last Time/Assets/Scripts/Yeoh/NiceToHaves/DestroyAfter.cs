using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    public bool startDestroyDelayOnEnable=true;
    public Vector2 destroyDelay = new(3, 4);

    [Header("Shrink Anim")]
    public List<GameObject> shrinkObjects = new();
    public Vector3 shrinkTo = Vector3.zero;
    public float shrinkTime=.5f;
    public bool ignoreTimescale;

    // ==================================================================================================================

    void OnEnable()
    {
        if(startDestroyDelayOnEnable) StartDestroyDelay();
    }

    // ==================================================================================================================

    public void StartDestroyDelay()
    {
        if(Destroying_crt!=null) StopCoroutine(Destroying_crt);
        Destroying_crt = StartCoroutine(Destroying());
    }

    Coroutine Destroying_crt;

    IEnumerator Destroying()
    {
        float delay = Random.Range(destroyDelay.x, destroyDelay.y);

        if(ignoreTimescale)
        yield return new WaitForSecondsRealtime(delay);
        else
        yield return new WaitForSeconds(delay);

        ShrinkAnim(shrinkTo, shrinkTime);

        if(ignoreTimescale)
        yield return new WaitForSecondsRealtime(shrinkTime);
        else
        yield return new WaitForSeconds(shrinkTime);

        Destroy(gameObject);
    }

    void ShrinkAnim(Vector3 to, float time)
    {
        foreach(var obj in shrinkObjects)
        {
            if(time>0) Tween.Scale(obj.transform, to, time, Ease.InOutSine, useUnscaledTime: ignoreTimescale);
            else obj.transform.localScale = to;
        }
    }

    // ==================================================================================================================
    
    public void DestroyNoAnim(float delay) => Destroy(gameObject, delay);
    public void DestroyNoAnim() => DestroyNoAnim(0);
}
