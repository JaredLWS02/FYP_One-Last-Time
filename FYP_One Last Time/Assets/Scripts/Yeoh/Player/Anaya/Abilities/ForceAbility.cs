using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceAbility : MonoBehaviour
{
    public GameObject owner;

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
    }

    // ============================================================================

    public BaseOverlap overlap;

    public void Force(bool pull, AbilitySO abilitySO)
    {
        overlap.Check();
        List<GameObject> objects = overlap.GetCurrentOverlaps();

        foreach(var obj in objects)
        {
            if (!obj) continue;

            Vector3 dir = (owner.transform.position - obj.transform.position).normalized;

            int mult = pull ? 1 : -1;

            EventM.OnForceReceived(obj, owner, abilitySO.magnitude, dir * mult, pull);
        }
    }
}
