using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GibSpawner : MonoBehaviour
{
    public GameObject owner;
    
    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.DeathEvent += OnDeath;
    }
    void OnDisable()
    {
        EventM.DeathEvent -= OnDeath;
    }

    // ============================================================================

    void OnDeath(GameObject victim, GameObject killer, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        if(victim!=owner) return;

        Spawn(hurtbox.knockback, contactPoint);
    }

    // ============================================================================

    public List<GameObject> gibs;
    public bool gibsKnockback=true;

    void Spawn(float force, Vector3 contactPoint)
    {
        foreach(var gib in gibs)
        {
            GameObject spawned = Instantiate(gib, owner.transform.position, Quaternion.identity);

            if(!gibsKnockback) return;

            EventM.OnTryKnockback(spawned, force, contactPoint);

            foreach(Transform child in spawned.transform)
            {
                EventM.OnTryKnockback(child.gameObject, force, contactPoint);
            }
        }
    }
}
