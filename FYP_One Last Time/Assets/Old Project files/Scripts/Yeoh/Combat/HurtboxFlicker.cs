using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtboxFlicker : MonoBehaviour
{
    Collider2D coll;
    
    void Awake()
    {
        coll=GetComponent<Collider2D>();
    }

    void OnEnable()
    {
        StartCoroutine(FlashingHurtbox());
    }

    public float flickerInterval=.2f;
    
    IEnumerator FlashingHurtbox()
    {
        while(true)
        {
            coll.enabled=true;
            yield return new WaitForSeconds(flickerInterval);
            coll.enabled=false;
        }
    }
}
