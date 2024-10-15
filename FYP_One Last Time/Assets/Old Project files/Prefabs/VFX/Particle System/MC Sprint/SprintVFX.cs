using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SprintVFX : MonoBehaviour
{
    public Rigidbody2D rb;
    ParticleSystem ps;

    void Awake()
    {
        ps=GetComponent<ParticleSystem>();
    }

    void OnEnable()
    {
        StartCoroutine(Footstepping());
    }
    
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public float minVelocity=2;

    bool IsSprinting()
    {
        return rb.velocity.magnitude>=minVelocity;
    }

    void Update()
    {
        if(IsSprinting())
        {
            ToggleParticles(true);
        }
        else ToggleParticles(false);
    }

    void ToggleParticles(bool toggle)
    {
        if(ps.isEmitting!=toggle)
        {
            if(toggle) ps.Play();
            else ps.Stop();
        }
    }
        
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public float fstInterval=.3f;

    IEnumerator Footstepping()
    {
        while(true)
        {
            yield return new WaitForSeconds(fstInterval);

            if(IsSprinting())
            {
                OnFootstep.Invoke();
            }
        }
    }

    public UnityEvent OnFootstep;
}
