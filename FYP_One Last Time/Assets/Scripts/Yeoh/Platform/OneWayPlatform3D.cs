using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(PlatformEffector3D))]
[RequireComponent(typeof(PassengerCarrier))]

public class OneWayPlatform3D : MonoBehaviour
{
    Collider coll;
    PlatformEffector3D effector;
    PassengerCarrier carrier;

    void Awake()
    {
        coll = GetComponent<Collider>();
        effector = GetComponent<PlatformEffector3D>();
        carrier = GetComponent<PassengerCarrier>();
    }

    // ============================================================================

    void OnEnable()
    {
        EventManager.Current.MoveYEvent += OnMoveY;
    }
    void OnDisable()
    {
        EventManager.Current.MoveYEvent -= OnMoveY;
    }

    // ============================================================================

    Dictionary<Collider, Coroutine> ignoringColl_crts = new();

    void OnMoveY(GameObject mover, float input_y)
    {
        if(input_y > -0.7f) return;
        // ignore if mover is not any passenger
        if(!carrier.IsPassenger(mover, out var passenger)) return;

        Collider coll = passenger.coll;

        if(ignoringColl_crts.TryGetValue(coll, out var crt))
        {
            if(crt!=null) StopCoroutine(crt);
        }

        ignoringColl_crts[coll] = StartCoroutine(IgnoringColl(coll));
    }

    // ============================================================================

    IEnumerator IgnoringColl(Collider coll)
    {
        if(coll)
        {
            effector.TryAddColliderToIgnore(coll);
            
            IgnoreColl(coll, true);
        }
        
        yield return new WaitForSeconds(.5f);

        if(coll)
        {
            IgnoreColl(coll, false);

            effector.TryRemoveColliderToIgnore(coll);
        }
    }

    void IgnoreColl(Collider targetColl, bool toggle)
    {
        Physics.IgnoreCollision(targetColl, coll, toggle);
    }
}
