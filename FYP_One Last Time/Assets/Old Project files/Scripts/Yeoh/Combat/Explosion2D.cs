using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion2D : MonoBehaviour
{
    public float dmg=5;
    public LayerMask layerMask;
    public float range=5;
    public float force=10;

    public void Explode()
    {
        Damage();
        Push();
        CameraManager.Current.Shake();
    }

    void Damage()
    {
        Collider2D[] others =  Physics2D.OverlapCircleAll(transform.position, range, layerMask);

        foreach(Collider2D other in others)
        {
            if(!other.isTrigger)
            {
                Rigidbody2D otherRb = other.attachedRigidbody;

                if(otherRb)
                {
                    float falloffMult = GetFalloffMult(transform.position, otherRb.transform.position, range);

                    falloffDmg = dmg * falloffMult;

                    contactPoint = other.ClosestPoint(transform.position);

                    EventManager.Current.OnHit(gameObject, otherRb.gameObject, CopyHurtInfo());
                }
            }
        }
    }

    float falloffDmg;
    Vector3 contactPoint;

    HurtInfo CopyHurtInfo()
    {
        HurtInfo info = new()
        {
            hurtSource = gameObject,
            dmg = falloffDmg,
            contactPoint = contactPoint,
        };

        return info;
    }

    void Push()
    {
        float range = this.range*1.5f;

        Collider2D[] others = Physics2D.OverlapCircleAll(transform.position, range, layerMask);

        foreach(Collider2D other in others)
        {
            Rigidbody2D otherRb = other.attachedRigidbody;

            if(otherRb)
            {
                Vector3 dir = (otherRb.transform.position - transform.position).normalized;

                float falloffMult = GetFalloffMult(transform.position, otherRb.transform.position, range);

                otherRb.velocity=Vector3.zero;
                
                otherRb.AddForce(force * dir * falloffMult, ForceMode2D.Impulse);
            }
        }
    }

    float GetFalloffMult(Vector3 from, Vector3 to, float range)
    {
        float distance = Vector3.Distance(from, to);

        return 1 - (distance/range);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, .5f, 0, .5f);
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
