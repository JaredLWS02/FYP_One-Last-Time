using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DropTable))]
[RequireComponent(typeof(HurtScript))]

public class DeathDrop : MonoBehaviour
{
    DropTable table;

    void Awake()
    {
        table = GetComponent<DropTable>();
    }

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
        if(victim!=gameObject) return;

        table.Drop();
    }

}
