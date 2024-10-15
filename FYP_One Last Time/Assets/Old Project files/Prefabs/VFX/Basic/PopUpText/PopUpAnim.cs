using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpAnim : MonoBehaviour
{
    Rigidbody rb;

    public float animIn=.5f, animWait=.5f, animOut=.5f;
    public bool destroyOnFinish=false;

    Vector3 defScale;

    void Awake()
    {
        rb=GetComponent<Rigidbody>();

        defScale = transform.localScale;
    }
    
    void OnEnable()
    {
        animatingRt = StartCoroutine(Animating());
    }
    void OnDisable()
    {
        if(animatingRt!=null) StopCoroutine(animatingRt);
    }

    Coroutine animatingRt;
    IEnumerator Animating()
    {
        transform.localScale = Vector3.zero;

        LeanTween.scale(gameObject, defScale, animIn).setEaseOutElastic().setIgnoreTimeScale(true);

        yield return new WaitForSeconds(animIn + animWait);

        LeanTween.scale(gameObject, Vector3.zero, animOut).setEaseInOutSine().setIgnoreTimeScale(true);

        yield return new WaitForSeconds(animOut);

        if(destroyOnFinish) Destroy(gameObject);
        else gameObject.SetActive(false);
    }

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
