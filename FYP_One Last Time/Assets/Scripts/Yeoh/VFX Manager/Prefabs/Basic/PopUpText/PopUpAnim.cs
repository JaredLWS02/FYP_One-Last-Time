using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class PopUpAnim : MonoBehaviour
{
    Rigidbody rb;

    public float inTime=.5f;
    public float waitTime=.5f;
    public float outTime=.5f;

    public bool ignoreTime=true;

    public bool destroyOnFinish=false;

    Vector3 defScale;

    void Awake()
    {
        rb=GetComponent<Rigidbody>();

        defScale = transform.localScale;
    }

    // ==================================================================================================================
    
    void OnEnable()
    {
        animating_crt = StartCoroutine(Animating());
    }
    void OnDisable()
    {
        if(animating_crt!=null) StopCoroutine(animating_crt);
    }

    // ==================================================================================================================

    Tween animTween;

    Coroutine animating_crt;
    
    IEnumerator Animating()
    {
        transform.localScale = Vector3.zero;

        animTween.Stop();
        animTween = Tween.Scale(transform, defScale, inTime, Ease.OutElastic, useUnscaledTime: ignoreTime);

        yield return new WaitForSeconds(inTime + waitTime);

        animTween.Stop();
        animTween = Tween.Scale(transform, Vector3.zero, outTime, Ease.InOutSine, useUnscaledTime: ignoreTime);

        yield return new WaitForSeconds(outTime);

        if(destroyOnFinish) Destroy(gameObject);
        else gameObject.SetActive(false);
    }

    // ==================================================================================================================

    public void Push(Vector3 force)
    {
        Vector3 randForce = new Vector3
        (
            Random.Range(-force.x, force.x),
            Random.Range(force.y*.5f, force.y),
            Random.Range(-force.z, force.z)
        );

        rb.AddForce(randForce, ForceMode.Impulse);
    }
}
