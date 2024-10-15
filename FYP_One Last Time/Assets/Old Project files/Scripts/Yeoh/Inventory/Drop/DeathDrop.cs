using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DropTable))]

public class DeathDrop : MonoBehaviour
{
    DropTable table;

    void Awake()
    {
        table=GetComponent<DropTable>();
    }

    // Event Manager ============================================================================
    
    void OnEnable()
    {
        EventManager.Current.DeathEvent += OnDeath;
    }
    void OnDisable()
    {
        EventManager.Current.DeathEvent -= OnDeath;
    }
    
    void OnDeath(GameObject victim, GameObject killer, HurtInfo hurtInfo)
    {
        if(victim!=gameObject) return;

        table.Drop();
    }

    // ============================================================================
}
