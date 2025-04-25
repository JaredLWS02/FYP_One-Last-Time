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

    public List<GameObject> gibs = new();

    public bool followRotation=true;

    public bool gibsKnockback=true;

    void Spawn(float force, Vector3 contactPoint)
    {
        foreach(var gib in gibs)
        {
            var rotation = followRotation ? owner.transform.rotation : Quaternion.identity;

            GameObject spawned = Instantiate(gib, owner.transform.position, rotation);

            if(!gibsKnockback) return;

            EventM.OnTryKnockback(spawned, force, contactPoint, true);

            foreach(Transform child in spawned.transform)
            {
                EventM.OnTryKnockback(child.gameObject, force, contactPoint, true);
            }
        }
    }

    // ============================================================================

    [Header("Optional")]
    public Transform simpleContactPoint;

    public void SpawnSimple(float force) => Spawn(force, simpleContactPoint.position);
}
