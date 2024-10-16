using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    public bool startDestroyDelayOnEnable=true;
    public Vector2 destroyDelay = new Vector2(3, 4);

    [Header("Shrink Anim")]
    public List<GameObject> shrinkObjects = new();
    public Vector3 shrinkTo = Vector3.zero;
    public float shrinkTime=.5f;
    public bool ignoreTime;

    // ==================================================================================================================

    void OnEnable()
    {
        if(startDestroyDelayOnEnable) StartDestroyDelay();
    }

    // ==================================================================================================================

    public void StartDestroyDelay()
    {
        float delay = Random.Range(destroyDelay.x, destroyDelay.y);

        ShrinkAnim(shrinkTo, shrinkTime, delay);

        Destroy(gameObject, delay+shrinkTime);
    }

    void ShrinkAnim(Vector3 to, float time, float delay=0)
    {
        List<GameObject> objectsToShrink = new();

        if(shrinkObjects.Count>0)
        {
            objectsToShrink = shrinkObjects;
        }
        else
        {
            objectsToShrink.Add(gameObject);
        }
        
        foreach(GameObject obj in objectsToShrink)
        {
            Tween.Scale(obj.transform, to, time, Ease.InOutSine, startDelay: delay, useUnscaledTime: ignoreTime);
        }
    }

    public void DestroyNoAnim()
    {
        Destroy(gameObject);
    }
}
