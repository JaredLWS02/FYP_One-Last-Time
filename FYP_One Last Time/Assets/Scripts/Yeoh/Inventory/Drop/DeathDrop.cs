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
    
    void OnEnable()
    {
        EventManager.Current.DeathEvent += OnDeath;
    }
    void OnDisable()
    {
        EventManager.Current.DeathEvent -= OnDeath;
    }
    
    // ============================================================================

    void OnDeath(GameObject victim, GameObject killer, HurtboxSO attack, Vector3 contactPoint)
    {
        if(victim!=gameObject) return;

        table.Drop();
    }

}
