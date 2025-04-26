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

    public bool ignoreTime;

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

    void FixedUpdate()
    {
        rb.gameObject.transform.up = rb.velocity.normalized;
    }

    // ==================================================================================================================

    Tween animTween;

    Coroutine animating_crt;
    
    IEnumerator Animating()
    {
        transform.localScale = Vector3.zero;

        animTween.Stop();
        if(inTime>0) animTween = Tween.Scale(transform, defScale, inTime, Ease.OutElastic, useUnscaledTime: ignoreTime);
        else transform.localScale = defScale;

        yield return new WaitForSeconds(inTime + waitTime);

        animTween.Stop();
        if(outTime>0) animTween = Tween.Scale(transform, Vector3.zero, outTime, Ease.InOutSine, useUnscaledTime: ignoreTime);
        else transform.localScale = Vector3.zero;

        yield return new WaitForSeconds(outTime);

        if(destroyOnFinish) Destroy(gameObject);
        else gameObject.SetActive(false);
    }

    // ==================================================================================================================

    public void Push(Vector3 force)
    {
        Vector3 randForce = new(
            Random.Range(-force.x*.5f, force.x*.5f),
            Random.Range(force.y, force.y),
            Random.Range(-force.z*.5f, force.z*.5f)
        );

        rb.AddForce(randForce, ForceMode.Impulse);
    }
}
