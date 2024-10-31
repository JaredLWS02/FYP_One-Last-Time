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

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.MoveEvent += OnMove;
    }
    void OnDisable()
    {
        EventM.MoveEvent -= OnMove;
    }

    // ============================================================================

    void OnMove(GameObject mover, Vector2 input)
    {
        if(input.y > -0.7f) return;
        // ignore if mover is not any passenger
        if(!carrier.IsPassenger(mover, out var passenger)) return;

        Collider coll = passenger.coll;

        TryStopCoroutine(coll);

        ignoringColl_crts[coll] = StartCoroutine(IgnoringColl(coll));
    }

    // ============================================================================

    Dictionary<Collider, Coroutine> ignoringColl_crts = new();

    void TryStopCoroutine(Collider coll)
    {
        if(ignoringColl_crts.TryGetValue(coll, out var crt))
        {
            if(crt!=null) StopCoroutine(crt);
        }
    }

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
